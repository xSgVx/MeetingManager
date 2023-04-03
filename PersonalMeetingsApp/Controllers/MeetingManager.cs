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

        private void EditTimeMeeting(string s, Operation editTimeOperation)
        {
            try
            {
                (DateTime, int) newDateTime = TryGetDataFromMeetingString(s, editTimeOperation);
                var oldMeeting = _meetings.ElementAt(newDateTime.Item2);
                Meeting editedMeeting = new Meeting(oldMeeting);

                if (editTimeOperation == Operation.EditStart)
                {
                    editedMeeting.EditStartTime(newDateTime.Item1);
                }

                if (editTimeOperation == Operation.EditEnd)
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
                (int, int) notifyAndId = TryGetDataFromMeetingString(s, Operation.EditNotify);
                var toEditMeeting = _meetings.ElementAt(notifyAndId.Item2);
                toEditMeeting.EditNotifyTime(notifyAndId.Item1);

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
                int meetId = TryGetDataFromMeetingString(s, Operation.Remove);
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

        public void ShowDayMeetings(string s)
        {
            DateTime date = TryGetDataFromMeetingString(s, Operation.Show);

            MessagesHandler?.Invoke(GetMeetingsString(date), MessageStatus.Info);
        }

        public void ShowAllMeetings()
        {
            MessagesHandler?.Invoke(GetMeetingsString(), MessageStatus.Info);
        }

        private dynamic TryGetDataFromMeetingString(string s, Operation operation)
        {
            var arrS = s.Trim().Split(' ');

            switch (operation)
            {
                case Operation.Add:
                    {
                        if (arrS.Length == 4)   //date, startTime, duration, notification
                        {
                            return TryGetDataTuple(arrS, ParseOptions.DateTime_Duration_Notification, true);
                        }

                        if (arrS.Length == 3)   //date, startTime, duration
                        {
                            return TryGetDataTuple(arrS, ParseOptions.DateTime_Duration, true);
                        }
                    }
                    break;
                case Operation.EditStart or Operation.EditEnd:
                    {
                        if (arrS.Length == 3)  //date, startTime or endTime, meetId
                        {
                            return TryGetDataTuple(arrS, ParseOptions.DateTime_MeetingId, true);
                        }
                    }
                    break;
                case Operation.EditNotify:
                    {
                        if (arrS.Length == 2)  //duration, meetId
                        {
                            return TryGetDataTuple(arrS, ParseOptions.DateTime_Duration_Notification);
                        }
                    }
                    break;
                case Operation.Show:
                    if (arrS.Length == 2)  //date, time
                    {
                        return TryGetDataTuple(arrS, ParseOptions.DateTime);
                    }
                    break;
                case Operation.Remove:
                    if (arrS.Length == 1)  //id
                    {
                        return TryGetDataTuple(arrS, ParseOptions.MeetingId);
                    }
                    break;

                default:
                    break;
            }

            throw new Exception(Messages.InputError);
        }

        private dynamic TryGetDataTuple(string[] dataArr, ParseOptions parseOptions, bool needValidateDate = false)
        {
            DateTime dateTime;
            int duration, notification, id;

            if (!InputDateCorrect(dataArr[0] + " " + dataArr[1], out dateTime, needValidateDate))
            {
                throw new Exception(Messages.EnteredDateError);
            }

            switch (parseOptions)
            {
                case ParseOptions.DateTime_Duration_Notification:
                    {
                        if (int.TryParse(dataArr[2], out duration) &&
                            int.TryParse(dataArr[3], out notification))
                        {
                            return (dateTime, duration, notification);
                        }

                        break;
                    }
                case ParseOptions.DateTime_Duration:
                    {
                        if (int.TryParse(dataArr[2], out duration))
                        {
                            return (dateTime, duration, 15);
                        }

                        break;
                    }
                case ParseOptions.DateTime_MeetingId:
                    {
                        if (int.TryParse(dataArr[2], out id))
                        {
                            return (dateTime, id);
                        }

                        break;
                    }
                case ParseOptions.Notify_MeetingId:
                    {
                        if (int.TryParse(dataArr[0], out notification) &&
                            int.TryParse(dataArr[1], out id))
                        {
                            return (notification, id);
                        }

                        break;
                    }
                case ParseOptions.DateTime:
                    {
                        return dateTime;
                    }
                case ParseOptions.MeetingId:
                    {
                        if (int.TryParse(dataArr[1], out id))
                        {
                            return id;
                        }

                        break;
                    }
            }

            throw new Exception(Messages.DataParseError);
        }

        private bool InputDateCorrect(string stringDateTime, out DateTime dateTime, bool needValidate = false)
        {
            if (DateTime.TryParse(stringDateTime, out dateTime))
            {
                if (needValidate)
                {
                    if (dateTime > DateTime.Today)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

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

        private string GetMeetingsString(DateTime? date = null)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < _meetings.Count; i++)
            {
                if (date != null)
                {
                    if (_meetings[i].StartTime.Day == date.Value.Day)
                    {
                        sb.Append($"Встреча №{i}:{Environment.NewLine}" +
                                  $"{_meetings[i].ToString()}");
                    }
                }
                else
                {
                    sb.Append($"Встреча №{i}:{Environment.NewLine}" +
                              $"{_meetings[i].ToString()}");
                }

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

    internal enum ParseOptions
    {
        DateTime_Duration_Notification,
        DateTime_Duration,
        DateTime_MeetingId,
        Notify_MeetingId,
        DateTime,
        MeetingId
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
