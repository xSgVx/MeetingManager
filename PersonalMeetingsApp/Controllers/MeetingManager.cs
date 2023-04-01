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
                var parsedData = TryParseMeetingString(s);
                var meeting = new Meeting(parsedData.Item1, parsedData.Item2,
                    parsedData.Item3);

                TryAddMeeting(meeting);
            }
            catch
            {
                MessagesHandler?.Invoke(Messages.DateParseError,
                    MessageStatus.Error);
            }
        }

        public void EditMeeting(int id)
        {

        }

        public void DeleteMeeting(int id)
        {
            var toRemoveMeeting = _meetings.ElementAt(id);
            _meetings.Remove(toRemoveMeeting);

            MessagesHandler?.Invoke(Messages.MeetingRemindInfo + Environment.NewLine +
                toRemoveMeeting.ToString(), MessageStatus.Info);
        }

        private (DateTime, int, int) TryParseMeetingString(string s)
        {
            DateTime dateTime;
            int duration, notification;

            var arrS = s.Trim().Split(' ');

            if (arrS.Length == 3)   //date, startTime, duration
            {
                if (DateTime.TryParse(arrS[0] + " " + arrS[1], out dateTime) &&
                    dateTime > DateTime.Today &&
                    int.TryParse(arrS[2], out duration))
                {
                    return (dateTime, duration, 15);
                }
            }

            if (arrS.Length == 4)   //date, startTime, duration, notification
            {
                if (DateTime.TryParse(arrS[0] + " " + arrS[1], out dateTime) &&
                    dateTime > DateTime.Today &&
                    int.TryParse(arrS[2], out duration) &&
                    int.TryParse(arrS[3], out notification))
                {
                    return (dateTime, duration, notification);
                }
            }

            throw new Exception();
        }

        //так же как вариант можно реализовать через events
        private async Task CheckForMeeting()
        {
            while (true)
            {
                if (_meetings.Any())
                {
                    var allToNotifyMeetings = _meetings?.Where(m => m.IsNotified == false &&                     //не уведомлено
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


        private void TryAddMeeting(IMeeting meeting)
        {
            if (!_meetings.Any() || !HasIntersections(meeting))
            {
                _meetings.Add(meeting);
                MessagesHandler?.Invoke(Messages.AddMeetingSuccess + Environment.NewLine +
                    meeting.ToString(), MessageStatus.Success);
            }
            else
            {
                MessagesHandler?.Invoke(Messages.IntersectionError, MessageStatus.Error);
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
                sb.Append($"Встреча #{i + 1}:{Environment.NewLine}" +
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
