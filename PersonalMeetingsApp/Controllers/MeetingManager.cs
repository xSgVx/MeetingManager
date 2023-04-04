using PersonalMeetingsApp.Models;
using PersonalMeetingsApp.Utility;
using System.Text;

namespace PersonalMeetingsApp.Controllers
{
    internal class MeetingManager
    {
        public IList<IMeeting> Meetings => _meetings;
        private readonly List<IMeeting> _meetings = null!;

        public delegate void MessageHandler(string message, MessageStatus messageStatus);
        public event MessageHandler? MessagesHandler;

        private const int defaultNotifTime = 15;

        public MeetingManager()
        {
            _meetings = new List<IMeeting>();

            Task.Run(MeetingNotifier);
            Task.Run(StatusUpdater);
        }

        public void AddMeeting(string s)
        {
            try
            {
                (DateTime date, int duration, int notif) parsedData = TryGetDataFromString(s, Operation.Add);

                var meeting = new Meeting(parsedData.date, parsedData.duration,
                    parsedData.notif);

                if (TryAddMeeting(meeting))
                {
                    MessagesHandler?.Invoke(Messages.AddMeetingSuccess + Environment.NewLine +
                        meeting.ToString(), MessageStatus.Success);

                    _meetings.Sort();
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

        public void EditStartTimeMeeting(string s) => EditTimeMeeting(s, Operation.EditStart);

        public void EditEndTimeMeeting(string s) => EditTimeMeeting(s, Operation.EditEnd);

        private void EditTimeMeeting(string s, Operation editTimeOperation)
        {
            try
            {
                (DateTime date, int meetId) newDateTime_Id = TryGetDataFromString(s, editTimeOperation);
                var oldMeeting = _meetings.ElementAt(newDateTime_Id.meetId);
                Meeting editedMeeting = new(oldMeeting);

                if (editTimeOperation == Operation.EditStart)
                {
                    editedMeeting.EditStartTime(newDateTime_Id.date);
                }

                if (editTimeOperation == Operation.EditEnd)
                {
                    editedMeeting.EditEndTime(newDateTime_Id.date);
                }

                _meetings.Remove(oldMeeting);

                if (TryAddMeeting(editedMeeting))   //нет пересечений
                {
                    MessagesHandler?.Invoke(Messages.EditMeetingSuccess + Environment.NewLine +
                        editedMeeting.ToString(), MessageStatus.Success);

                    _meetings.Sort();
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
                (int, int) notifyTime_Id = TryGetDataFromString(s, Operation.EditNotify);
                var toEditMeeting = _meetings.ElementAt(notifyTime_Id.Item2);
                toEditMeeting.EditNotifyTime(notifyTime_Id.Item1);

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
                int meetId = TryGetDataFromString(s, Operation.Remove);
                var toRemoveMeeting = _meetings.ElementAt(meetId);
                _meetings.Remove(toRemoveMeeting);

                MessagesHandler?.Invoke(Messages.RemoveMeetingSuccess, MessageStatus.Info);
            }
            catch
            {
                MessagesHandler?.Invoke(Messages.DataParseError, MessageStatus.Error);
            }
        }

        public void ShowDayMeetings(string s)
        {
            try
            {
                DateOnly date = TryGetDataFromString(s, Operation.Show);

                MessagesHandler?.Invoke(GetMeetingsString(date), MessageStatus.Info);
            }
            catch
            {
                MessagesHandler?.Invoke(Messages.DataParseError, MessageStatus.Error);
            }

        }

        public void ShowAllMeetings()
        {
            MessagesHandler?.Invoke(GetMeetingsString(), MessageStatus.Info);
        }

        public void ExportMeetings(string s, bool append = false)
        {
            try
            {
                (DateOnly date, string path) datePath = TryGetDataFromString(s, Operation.Export);

                using (StreamWriter sw = new(datePath.path, append))
                {
                    sw.WriteLine(GetMeetingsString(datePath.date));
                }

                MessagesHandler?.Invoke(Messages.MeetingsExported, MessageStatus.Success);
            }
            catch (Exception e)
            {
                MessagesHandler?.Invoke(Messages.MeetingsExportError + e.Message, MessageStatus.Error);
            }
        }

        private dynamic TryGetDataFromString(string s, Operation operation)
        {
            var arrS = s.Trim().Split(' '); //проверка массива на правильность входных данных

            switch (operation)
            {
                case Operation.Add:
                    {
                        if (arrS.Length == 4)   //date, startTime, duration, notification
                        {
                            return TryGetDataTuple(arrS, ParseOptions.DateTime_Duration_Notification);
                        }

                        if (arrS.Length == 3)   //date, startTime, duration
                        {
                            return TryGetDataTuple(arrS, ParseOptions.DateTime_Duration);
                        }
                    }
                    break;
                case Operation.EditStart or Operation.EditEnd:
                    {
                        if (arrS.Length == 3)  //date, startTime or endTime, meetId
                        {
                            return TryGetDataTuple(arrS, ParseOptions.DateTime_MeetingId);
                        }
                    }
                    break;
                case Operation.EditNotify:
                    {
                        if (arrS.Length == 2)  //duration, meetId
                        {
                            return TryGetDataTuple(arrS, ParseOptions.Notify_MeetingId);
                        }
                    }
                    break;
                case Operation.Show:
                    if (arrS.Length == 1)  //dateonly
                    {
                        return TryGetDataTuple(arrS, ParseOptions.DateOnly);
                    }
                    break;
                case Operation.Remove:
                    if (arrS.Length == 1)  //id
                    {
                        return TryGetDataTuple(arrS, ParseOptions.MeetingId);
                    }
                    break;
                case Operation.Export:
                    if (arrS.Length >= 2)  //dateonly, path
                    {
                        return TryGetDataTuple(arrS, ParseOptions.DateOnly_Path);
                    }
                    break;
            }

            throw new Exception(Messages.InputError);
        }

        private dynamic TryGetDataTuple(string[] dataArr, ParseOptions parseOptions)
        {
            DateTime dateTime;
            int duration, notification, id;

            switch (parseOptions)
            {
                case ParseOptions.DateTime_Duration_Notification:
                    {
                        if (TryParseDate(dataArr[0] + " " + dataArr[1], out dateTime) &&
                            int.TryParse(dataArr[2], out duration) &&
                            int.TryParse(dataArr[3], out notification))
                        {
                            return (dateTime, duration, notification);
                        }

                        break;
                    }
                case ParseOptions.DateTime_Duration:
                    {
                        if (TryParseDate(dataArr[0] + " " + dataArr[1], out dateTime) &&
                            int.TryParse(dataArr[2], out duration))
                        {
                            return (dateTime, duration, defaultNotifTime);
                        }

                        break;
                    }
                case ParseOptions.DateTime_MeetingId:
                    {
                        if (TryParseDate(dataArr[0] + " " + dataArr[1], out dateTime) &&
                           int.TryParse(dataArr[2], out id))
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
                case ParseOptions.DateOnly:
                    {
                        if (DateOnly.TryParse(dataArr[0], out DateOnly dateOnly))
                        {
                            return dateOnly;
                        }

                        break;
                    }
                case ParseOptions.MeetingId:
                    {
                        if (int.TryParse(dataArr[0], out id))
                        {
                            return id;
                        }

                        break;
                    }
                case ParseOptions.DateOnly_Path:
                    {
                        if (DateOnly.TryParse(dataArr[0], out DateOnly dateOnly))
                        {
                            string path = string.Join("", dataArr.Skip(1));
                            if (!string.IsNullOrEmpty(Path.GetDirectoryName(path)))
                            {
                                return (dateOnly, path);
                            }
                        }

                        break;
                    }
            }

            throw new Exception(Messages.DataParseError);
        }

        private bool TryParseDate(string stringDateTime, out DateTime dateTime)
        {
            if (DateTime.TryParse(stringDateTime, out dateTime) && dateTime > DateTime.Now)
            {
                return true;
            }
            else
            {
                throw new Exception(Messages.EnteredDateError);
            }

            throw new Exception(Messages.DataParseError);
        }

        private readonly object _locker = new();
        private Task MeetingNotifier()
        {
            while (true)
            {
                lock (_locker)
                {
                    if (_meetings != null && _meetings.Any())
                    {
                        var allToNotifyMeetings = _meetings?.Where(m => m.IsNotified == false &&
                                                                  (DateTime.Now.Day == m.StartTime.Day) &&
                                                                  ((m.StartTime - DateTime.Now).Minutes < m.NotifyMinutes));

                        if (allToNotifyMeetings?.Count() > 0)
                        {
                            foreach (var meet in allToNotifyMeetings)
                            {
                                MessagesHandler?.Invoke(Messages.MeetingRemindInfo + Environment.NewLine +
                                    meet.ToString() + Messages.EmptySpace, MessageStatus.Info);

                                meet.IsNotified = true;
                            }
                        }
                    }
                }

                Thread.Sleep(1000);     //1 sec
            }
        }

        private readonly object _locker2 = new();
        private Task StatusUpdater()
        {
            while (true)
            {
                lock(_locker2)
                {
                    if (_meetings != null && _meetings.Any())
                    {
                        var meetingsWithOldStatus = _meetings?.Where(m => (DateTime.Now > m.EndTime) &&
                                                                    (m.MeetingStatus != MeetingStatus.Ended));

                        if (meetingsWithOldStatus?.Count() > 0)
                        {
                            foreach (var meet in meetingsWithOldStatus)
                            {
                                meet.MeetingStatus = MeetingStatus.Ended;
                            }
                        }
                    }
                }
                
                Thread.Sleep(1000);     //1 sec
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

        private string GetMeetingsString(DateOnly? date = null)
        {
            if (!_meetings.Any() ||
                (date != null &&
                !_meetings.Any(m => m.StartTime.Day == date.Value.Day)))
            {
                return Messages.NoMeetings;
            }

            var sb = new StringBuilder();

            for (int i = 0; i < _meetings.Count; i++)
            {
                if (date != null)
                {
                    if (_meetings[i].StartTime.Day == date.Value.Day)
                    {
                        sb.Append($"\nВстреча №{i}:\n" +
                                  $"{_meetings[i].ToString()}\n");
                    }
                }
                else
                {
                    sb.Append($"\nВстреча №{i}:\n" +
                              $"{_meetings[i].ToString()}\n");
                }

            }

            return sb.ToString();
        }
    }
}
