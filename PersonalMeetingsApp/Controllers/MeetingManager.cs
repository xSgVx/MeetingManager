using PersonalMeetingsApp.Models;
using PersonalMeetingsApp.Utility;
using System.Text;

namespace PersonalMeetingsApp.Controllers
{
    internal class MeetingManager
    {
        public IList<IMeeting> Meetings => _meetings;
        private readonly List<IMeeting> _meetings = new();

        public delegate void MessageHandler(string message, MessageStatus messageStatus);
        public event MessageHandler? MessagesHandler;

        public MeetingManager()
        {
            Task.Run(MeetingNotifier);
            Task.Run(StatusUpdater);
        }

        public void AddMeeting(string s)
        {
            try
            {
                (DateTime date, int duration, int notif) parsedData = ConsoleParser.TryGetDataFromString(s, Operation.Add);

                var meeting = new Meeting(parsedData.date, parsedData.duration,
                    parsedData.notif);

                if (TryAddMeeting(meeting))
                {
                    MessagesHandler?.Invoke(Messages.AddMeetingSuccess + Environment.NewLine +
                        meeting.ToString(), MessageStatus.Success);
                }
            }
            catch (Exception e)
            {
                MessagesHandler?.Invoke(e.Message, MessageStatus.Error);
            }
        }

        public void EditStartTimeMeeting(string s) => EditTimeMeeting(s, Operation.EditStart);

        public void EditEndTimeMeeting(string s) => EditTimeMeeting(s, Operation.EditEnd);

        private void EditTimeMeeting(string s, Operation editTimeOperation)
        {
            try
            {
                (DateTime date, int meetId) newDateTime_Id = ConsoleParser.TryGetDataFromString(s, editTimeOperation);
                var oldMeeting = _meetings.ElementAt(newDateTime_Id.meetId);
                Meeting editedMeeting = new(oldMeeting);

                if (editTimeOperation == Operation.EditStart)
                {
                    if (!editedMeeting.TryEditStartTime(newDateTime_Id.date))
                    {
                        MessagesHandler?.Invoke(Messages.EnteredDateError, MessageStatus.Error);
                        return;
                    }
                }

                if (editTimeOperation == Operation.EditEnd)
                {
                    if (!editedMeeting.TryEditEndTime(newDateTime_Id.date))
                    {
                        MessagesHandler?.Invoke(Messages.EnteredDateError, MessageStatus.Error);
                        return;
                    }
                }

                _meetings.Remove(oldMeeting);

                if (TryAddMeeting(editedMeeting))   //нет пересечений
                {
                    MessagesHandler?.Invoke(Messages.EditMeetingSuccess + Environment.NewLine +
                        editedMeeting.ToString(), MessageStatus.Success);
                }
                else    //есть пересечения, возвращаем старое 
                {
                    _meetings.Add(oldMeeting);
                }
            }
            catch (Exception e)
            {
                MessagesHandler?.Invoke(e.Message, MessageStatus.Error);
            }
        }

        public void EditNotificationTimeMeeting(string s)
        {
            try
            {
                (int, int) notifyTime_Id = ConsoleParser.TryGetDataFromString(s, Operation.EditNotify);
                var toEditMeeting = _meetings.ElementAt(notifyTime_Id.Item2);
                toEditMeeting.EditNotifyTime(notifyTime_Id.Item1);

                MessagesHandler?.Invoke(Messages.EditMeetingNotifySuccess + Environment.NewLine +
                    toEditMeeting.ToString(), MessageStatus.Info);
            }
            catch (Exception e)
            {
                MessagesHandler?.Invoke(e.Message, MessageStatus.Error);
            }
        }

        public void RemoveMeeting(string s)
        {
            try
            {
                int meetId = ConsoleParser.TryGetDataFromString(s, Operation.Remove);
                var toRemoveMeeting = _meetings.ElementAt(meetId);
                _meetings.Remove(toRemoveMeeting);

                MessagesHandler?.Invoke(Messages.RemoveMeetingSuccess, MessageStatus.Info);
            }
            catch (Exception e)
            {
                MessagesHandler?.Invoke(e.Message, MessageStatus.Error);
            }
        }

        public void ShowDayMeetings(string s)
        {
            try
            {
                DateOnly date = ConsoleParser.TryGetDataFromString(s, Operation.Show);

                MessagesHandler?.Invoke(GetMeetingsString(date), MessageStatus.Info);
            }
            catch (Exception e)
            {
                MessagesHandler?.Invoke(e.Message, MessageStatus.Error);
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
                (DateOnly date, string path) datePath = ConsoleParser.TryGetDataFromString(s, Operation.Export);

                var meetingsString = GetMeetingsString(datePath.date);
                if (meetingsString == Messages.NoMeetings)      //встреч для экспорта нет, создавать файл не нужно
                {
                    MessagesHandler?.Invoke(meetingsString, MessageStatus.Info);
                    return;
                }

                using (StreamWriter sw = new(datePath.path, append))    //встречи есть, пишем в файл
                {
                    sw.WriteLine(meetingsString);
                }

                MessagesHandler?.Invoke(Messages.MeetingsExported, MessageStatus.Success);
            }
            catch (Exception e)
            {
                MessagesHandler?.Invoke(Messages.MeetingsExportError + e.Message, MessageStatus.Error);
            }
        }

        private bool TryAddMeeting(IMeeting meeting)
        {
            if (!_meetings.Any() || !HasIntersections(meeting))
            {
                _meetings.Add(meeting);
                _meetings.Sort();
                return true;
            }
            else
            {
                MessagesHandler?.Invoke(Messages.IntersectionError,
                    MessageStatus.Error);
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
                lock (_locker2)
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
    }
}
