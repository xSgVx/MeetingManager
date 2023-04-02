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
    enum ParseOptions
    {
        Date_Time_Duration_Notification,
        Date_Time_Duration,
        Date_Time_Id,
    }

    enum Crud
    {
        Add,
        Edit,
        Remove,
    }



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
                var parsedData = TryGetDataFromMeetingString(s, Crud.Add);

                var meeting = new Meeting(parsedData.Item1, parsedData.Item2,
                    parsedData.Item3);

                TryAddMeeting(meeting);
            }
            catch (Exception e)
            {
                MessagesHandler?.Invoke(e.Message,
                    MessageStatus.Error);
            }
        }

        //NOTE ДООФОРМИТЬ
        public void EditStartTimeMeeting(string s)
        {
            try
            {
                var newStartData = TryGetDataFromMeetingString(s, Crud.Edit);
                var oldMeeting = (Meeting)_meetings.ElementAt(newStartData.Item2);
                Meeting editedMeeting = new Meeting(oldMeeting);
                editedMeeting.EditStartTime(newStartData.Item1);

                _meetings.Remove(oldMeeting);
                TryAddMeeting(editedMeeting);

                MessagesHandler?.Invoke(Messages.EditMeetingSuccess + Environment.NewLine +
                    editedMeeting.ToString(), MessageStatus.Info);
            }
            catch (Exception e)
            {
                MessagesHandler?.Invoke(e.Message,
                    MessageStatus.Error);
            }
        }

        private (DateTime, int) TryParseNewTimeString(string s)
        {
            DateTime newTime;
            int id;

            var arrS = s.Trim().Split(' ');

            if (arrS.Length == 2)
            {
                if (DateTime.TryParse(arrS[1] + " " + arrS[2], out newTime) &&
                    newTime > DateTime.Today &&
                    int.TryParse(arrS[0], out id))
                {
                    return new(newTime, id);
                }
            }

            throw new Exception(Messages.InputError);
        }

        public void RemoveMeeting(int id)
        {
            var toRemoveMeeting = _meetings.ElementAt(id);
            _meetings.Remove(toRemoveMeeting);

            MessagesHandler?.Invoke(Messages.MeetingRemindInfo + Environment.NewLine +
                toRemoveMeeting.ToString(), MessageStatus.Info);
        }



        private dynamic TryGetDataFromMeetingString(string s, Crud crud)
        {
            var arrS = s.Trim().Split(' ');

            if (arrS.Length == 4)   //date, startTime, duration, notification
            {
                return TryGetDataTuple(arrS, ParseOptions.Date_Time_Duration_Notification);
            }

            if (arrS.Length == 3)
            {
                if (crud == Crud.Add)   //date, startTime, duration
                {
                    return TryGetDataTuple(arrS, ParseOptions.Date_Time_Duration);
                }

                if (crud == Crud.Edit)  //date, startTime, meetId
                {
                    return TryGetDataTuple(arrS, ParseOptions.Date_Time_Id);
                }
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
                MessagesHandler?.Invoke(Messages.AddMeetingSuccess + Environment.NewLine +
                    meeting.ToString(), MessageStatus.Success);

                return true;
            }
            else
            {
                throw new Exception(Messages.IntersectionError);
            }
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

        public string GetMeetingsString()
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
                sw.WriteLine(GetMeetingsString());
            }
        }



    }
}
