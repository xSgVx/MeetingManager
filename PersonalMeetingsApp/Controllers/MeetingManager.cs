using PersonalMeetingsApp.Models;
using PersonalMeetingsApp.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PersonalMeetingsApp.Controllers
{
    internal class MeetingManager
    {
        public IList<IMeeting> Meetings => _meetings;
        private IList<IMeeting> _meetings;

        public delegate void MessageHandler(string message);
        public event MessageHandler? MessagesHandler;

        public MeetingManager()
        {
            _meetings = new List<IMeeting>();
        }



        public void AddMeeting(string ddMMyyyy, string hhMM, 
            int duration, int notification = 15)
        {
            DateTime dateTime;

            if (DateTime.TryParse(ddMMyyyy + " " + hhMM, out dateTime)
                && dateTime > DateTime.Today)
            {
                var meeting = new Meeting(dateTime, duration, notification);
                TryAddMeeting(meeting);
            }
            else
            {
                MessagesHandler.Invoke(Messages.DateParseError);
            }
        }

        

        void TryAddMeeting(IMeeting meeting)
        {
            if (_meetings.Any())
            {
                if (!HasIntersections(meeting))
                {
                    _meetings.Add(meeting);
                    MessagesHandler.Invoke(Messages.AddMeetingSuccess);
                }
                else
                {
                    MessagesHandler.Invoke(Messages.IntersectionError);
                }
            }
            else
            {
                _meetings.Add(meeting);
            }
        }

        private bool HasIntersections(IMeeting newMeeting)
        {
            foreach (var existMeeting in _meetings)
            {
                if (!HasNoIntersections(existMeeting, newMeeting))
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasNoIntersections(IMeeting curMeeting, IMeeting toCheckMeeting)
        {
            if (curMeeting.StartTime.Date == toCheckMeeting.StartTime.Date)
            {
                if (curMeeting.StartTime > toCheckMeeting.EndTime || 
                    curMeeting.EndTime < toCheckMeeting.StartTime)
                {
                    return true;
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

        public void ExportMeetings(string outFile)
        {
            if (File.Exists(outFile))
            {
                File.Delete(outFile);
            }

            using (StreamWriter sw = new StreamWriter(outFile))
            {
                sw.WriteLine(GetMeetingsString());
            }
        }

    }
}
