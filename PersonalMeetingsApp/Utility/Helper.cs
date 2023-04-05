using PersonalMeetingsApp.Models;
using PersonalMeetingsApp.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalMeetingsApp.Utility
{
    internal static class Helper
    {
        public static string GetMeetingsString(List<IMeeting> meetings, DateOnly? date = null)
        {
            if (!meetings.Any() ||
            (date != null &&
                !meetings.Any(m => m.StartTime.Day == date.Value.Day)))
            {
                return Messages.NoMeetings;
            }

            var sb = new StringBuilder();

            for (int i = 0; i < meetings.Count; i++)
            {
                if (date != null)
                {
                    if (meetings[i].StartTime.Day == date.Value.Day)
                    {
                        sb.Append($"\nВстреча №{i}:\n" +
                                  $"{meetings[i].ToString()}\n");
                    }
                }
                else
                {
                    sb.Append($"\nВстреча №{i}:\n" +
                              $"{meetings[i].ToString()}\n");
                }

            }

            return sb.ToString();
        }

        public static bool TryParseDate(string stringDateTime, out DateTime dateTime)
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

        public static bool HasIntersections(IMeeting newMeeting, List<IMeeting> meetings)
        {
            if (meetings.Any())
            {
                foreach (var existMeeting in meetings)
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
            }

            return false;
        }


    }
}
