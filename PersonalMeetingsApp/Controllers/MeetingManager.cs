using PersonalMeetingsApp.Models;
using PersonalMeetingsApp.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PersonalMeetingsApp.Controllers
{
    internal class MeetingManager
    {
        public IList<IMeeting> Meetings => _meetings;
        private IList<IMeeting> _meetings;

        public delegate void MessageHandler(string message, MessageStatus messageStatus);
        public event MessageHandler? MessagesHandler;

        //public delegate void MeetingNotify(MeetingStatus meetingStatus);
        //public event MeetingNotify? MeetingsNotifier;

        public MeetingManager()
        {
            _meetings = new List<IMeeting>();

            Task.Run(CheckForMeeting);
        }

        public void AddMeeting(string s)
        {
            try
            {
                (DateTime, int, int) parsedData = TryGetDataFromMeetingString(s, Operation.Add);

                var meeting = new Meeting(parsedData.Item1, parsedData.Item2,
                    parsedData.Item3);

                if (TryAddMeeting(meeting))
                {
                    MessagesHandler?.Invoke(Messages.AddMeetingSuccess + Environment.NewLine +
                        meeting.ToString(), MessageStatus.Success);
                }
                else
                {
                    MessagesHandler?.Invoke(Messages.IntersectionError,
                        MessageStatus.Error);
                }
            }
            catch (Exception e)
            {
                MessagesHandler?.Invoke(e.Message,
                    MessageStatus.Error);
            }
        }

        public void EditStartTimeMeeting(string s)
        {
            EditTimeMeeting(s, Operation.EditStart);
        }

        public void EditEndTimeMeeting(string s)
        {
            EditTimeMeeting(s, Operation.EditEnd);
        }

        private void EditTimeMeeting(string s, Operation operation)
        {
            try
            {
                (DateTime, int) newDateTime = TryGetDataFromMeetingString(s, operation);
                var oldMeeting = _meetings.ElementAt(newDateTime.Item2);
                Meeting editedMeeting = new Meeting(oldMeeting);

                if (operation == Operation.EditStart)
                {
                    editedMeeting.EditStartTime(newDateTime.Item1);
                }

                if (operation == Operation.EditEnd)
                {
                    editedMeeting.EditEndTime(newDateTime.Item1);
                }

                _meetings.Remove(oldMeeting);

                if (TryAddMeeting(editedMeeting))   //нет пересечений
                {
                    MessagesHandler?.Invoke(Messages.EditMeetingSuccess + Environment.NewLine +
                        editedMeeting.ToString(), MessageStatus.Success);
                }
                else    //есть пересечения, возвращаем старое 
                {
                    MessagesHandler?.Invoke(Messages.IntersectionError,
                        MessageStatus.Error);

                    _meetings.Add(oldMeeting);
                }
            }
            catch (Exception e)
            {
                MessagesHandler?.Invoke(e.Message,
                    MessageStatus.Error);
            }
        }

        public void EditNotificationTimeMeeting(string s)
        {
            try
            {
                var arrS = s.Trim().Split(' ').Select(int.Parse).ToArray();
                var toEditMeeting = _meetings.ElementAt(arrS[1]);
                toEditMeeting.EditNotifyTime(arrS[0]);

                MessagesHandler?.Invoke(Messages.EditMeetingNotifySuccess + Environment.NewLine +
                    toEditMeeting.ToString(), MessageStatus.Info);
            }
            catch
            {
                MessagesHandler?.Invoke(Messages.DataParseError, MessageStatus.Error);
            }
        }

        public void RemoveMeeting(string s)
        {
            try
            {
                var meetId = int.Parse(s.Trim());
                var toRemoveMeeting = _meetings.ElementAt(meetId);
                _meetings.Remove(toRemoveMeeting);

                MessagesHandler?.Invoke(Messages.RemoveMeetingSuccess + Environment.NewLine +
                    toRemoveMeeting.ToString(), MessageStatus.Info);
            }
            catch
            {
                MessagesHandler?.Invoke(Messages.DataParseError, MessageStatus.Error);
            }
        }

        private dynamic TryGetDataFromMeetingString(string s, Operation operation)
        {
            var arrS = s.Trim().Split(' ');

            if (arrS.Length == 4 &&
                operation == Operation.Add)   //date, startTime, duration, notification
            {
                return TryGetDataTuple(arrS, ParseOptions.Date_Time_Duration_Notification);
            }

            if (arrS.Length == 3)
            {
                if (operation == Operation.Add)   //date, startTime, duration
                {
                    return TryGetDataTuple(arrS, ParseOptions.Date_Time_Duration);
                }

                if (operation == Operation.EditStart ||
                    operation == Operation.EditEnd)  //date, startTime or endTime, meetId
                {
                    return TryGetDataTuple(arrS, ParseOptions.Date_Time_Id);
                }
            }

            if (arrS.Length == 2)   //duration, meetId
            {
                return TryGetDataTuple(arrS, ParseOptions.Date_Time_Duration_Notification);
            }


            throw new Exception(Messages.InputError);
        }

        private dynamic TryGetDataTuple(string[] dataArr, ParseOptions parseOptions)
        {
            DateTime dateTime;
            int duration, notification, id;

            if (!InputDateCorrect(dataArr[0] + " " + dataArr[1], out dateTime))
            {
                throw new Exception(Messages.DataParseError);
            }

            switch (parseOptions)
            {
                case ParseOptions.Date_Time_Duration_Notification:
                    {
                        if (int.TryParse(dataArr[2], out duration) &&
                            int.TryParse(dataArr[3], out notification))
                        {
                            return (dateTime, duration, notification);
                        }

                        break;
                    }
                case ParseOptions.Date_Time_Duration:
                    {
                        if (int.TryParse(dataArr[2], out duration))
                        {
                            return (dateTime, duration, 15);
                        }

                        break;
                    }
                case ParseOptions.Date_Time_Id:
                    {
                        if (int.TryParse(dataArr[2], out id))
                        {
                            return (dateTime, id);
                        }

                        break;
                    }
            }

            throw new Exception(Messages.DataParseError);
        }

        private bool InputDateCorrect(string stringDateTime, out DateTime dateTime)
        {
            if (DateTime.TryParse(stringDateTime, out dateTime) &&
                            dateTime > DateTime.Today)
            {
                return true;
            }

            return false;
        }

        //так же как вариант можно реализовать через events
        //данный вариант не совсем thread-safe
        private async Task CheckForMeeting()
        {
            while (true)
            {
                if (_meetings.Any())
                {
                    var allToNotifyMeetings = _meetings?.Where(m => m.IsNotified == false &&
                                                              (DateTime.Now.Day == m.StartTime.Day) &&
                                                              ((DateTime.Now - m.StartTime).Minutes - m.NotifyMinutes) <= 0);

                    if (allToNotifyMeetings?.Count() > 0)
                    {
                        foreach (var meet in allToNotifyMeetings)
                        {
                            MessagesHandler?.Invoke(Messages.MeetingRemindInfo + Environment.NewLine +
                                meet.ToString(), MessageStatus.Info);

                            meet.IsNotified = true;
                        }
                    }
                }

                //Thread.Sleep(60000);    //every 1 min checking for notify
            }
        }

        private bool TryAddMeeting(IMeeting meeting)
        {
            if (!_meetings.Any() || !HasIntersections(meeting))
            {
                _meetings.Add(meeting);
                return true;
            }

            return false;
        }

        private bool HasIntersections(IMeeting newMeeting)
        {
            foreach (var existMeeting in _meetings)
            {
                if (existMeeting.StartTime.Date == newMeeting.StartTime.Date)
                {
                    if (existMeeting.StartTime > newMeeting.EndTime ||
                        existMeeting.EndTime < newMeeting.StartTime)
                    {
                        continue;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void ShowMeetings()
        {

        }

        private string GetAllMeetingsString()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < _meetings.Count; i++)
            {
                sb.Append($"Встреча №{i}:{Environment.NewLine}" +
                          $"{_meetings[i].ToString()}");
            }

            return sb.ToString();
        }

        public void ExportMeetings(string outFile, bool rewriteFile)
        {
            using (StreamWriter sw = new StreamWriter(outFile, !rewriteFile))
            {
                sw.WriteLine(GetAllMeetingsString());
            }
        }



    }

    internal enum ParseOptions
    {
        Date_Time_Duration_Notification,
        Date_Time_Duration,
        Date_Time_Id,
    }

    internal enum Operation
    {
        Add,
        EditStart,
        EditEnd,
        EditNotify,
        Show,
        Remove,
    }
}
