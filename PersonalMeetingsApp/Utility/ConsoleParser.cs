using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalMeetingsApp.Utility
{
    internal static class ConsoleParser
    {
        private const int defaultNotifTime = 15;

        public static dynamic TryGetDataFromString(string s, Operation operation)
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

                        if (arrS.Length == 3)   //date, startTime, duration , defaultNotification = 15
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

        private static dynamic TryGetDataTuple(string[] dataArr, ParseOptions parseOptions)
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

        private static bool TryParseDate(string stringDateTime, out DateTime dateTime)
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
    }
}
