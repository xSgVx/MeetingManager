using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalMeetingsApp.Utility
{
    internal static class Messages
    {
        internal const string IntersectionError = "\nError: meeting has intersection with other meetings\n";
        internal const string ErrorOnEditEndTime = "\nError: uncorrect time of meeting end\n";
        internal const string ErrorOnEditStartTime = "\nError: uncorrect time of meeting start\n";
        internal const string InputError = "\nError: input is not correct\n";
        internal const string DataParseError = "\nError: error while parsing input data\n";
        internal const string EnteredDateError = "\nError: entered date is not correct\n";
        internal const string MeetingsExportError = "\nError: export path not correct\n";
        internal const string MeetingEndedError = "\nError: meeting ended, cant change time\n";

        internal const string AddMeetingSuccess = "\nSuccess: meeting has been added\n";
        internal const string EditMeetingSuccess = "\nSuccess: meeting time has been changed\n";
        internal const string EditMeetingNotifySuccess = "\nSuccess: meeting notify time has been changed\n";
        internal const string RemoveMeetingSuccess = "\nSuccess: meeting has been deleted\n";
        internal const string MeetingsExported = "\nSuccess: meetings has been exported\n";

        //internal const string Success = "\n---------------------------------------------\n" +
        //    "Success" +
        //    "\n---------------------------------------------\n";

        internal const string MeetingRemindInfo = "\nInfo: next meeting soon, be ready!\n";

        internal const string NextActionButton = "\n---------------------------------------------\n" +
            "Press \"1\" to ADD new meeting,\n" +
            "Press \"2\" to EDIT START TIME of existing meeting,\n" +
            "Press \"3\" to EDIT END TIME of existing meeting,\n" +
            "Press \"4\" to EDIT NOTIFICATION TIME of existing meeting,\n" +
            "Press \"5\" to REMOVE existing meeting,\n" +
            "Press \"6\" to SHOW ALL meetings,\n" +
            "Press \"7\" to SHOW DAY meetings,\n" +
            "Press \"8\" to EXPORT meetings into file\n" +
            "Press \"ESC\" to close program" +
            "\n---------------------------------------------\n";

        internal const string EnterNewMeetingInfo = "\n---------------------------------------------\n" +
            "\nEnter meeting info: day, time, " +
            "meeting duration (in minutes),\n" +
            "notification time (is optional, default time is 15 minutes)\n" +
            "at format:\n" +
            "dd/MM/yyyy hh:MM MM MM or\n" +
            "dd/MM/yyyy hh:MM MM\n" +
            "For example: 01/01/2001 12:21 30 15 , where 30 is duration (in minutes),\n" +
            "15 minutes will be notification time" +
            "\n---------------------------------------------\n";

        internal const string EnterNewStartTime = $"\n---------------------------------------------\n" +
            "\nEnter new start time of chosen meeting\n" +
            "at format:\n" +
            "dd/MM/yyyy hh:MM meetingId\n" +
            "For example: 01/01/2001 12:21 5, where 12:21 is new start time, 5 is meetingId\n" +
            "(To know meetingID u can SHOW ALL or SHOW DAY meetings)\n" +
            "\n---------------------------------------------\n";

        internal const string EnterNewEndTime = $"\n---------------------------------------------\n" +
            "\nEnter new end time of chosen meeting\n" +
            "at format:\n" +
            "dd/MM/yyyy hh:MM meetingId\n" +
            "For example: 01/01/2001 12:21 5, where 12:21 is new end time, 5 is meetingId\n" +
            "(To know meetingID u can SHOW ALL or SHOW DAY meetings)\n" +
            "\n---------------------------------------------\n";

        internal const string EnterNotifyTimeMeetingID = $"\n---------------------------------------------\n" +
            "\nEnter new notify time and meeting id to remove\n" +
            "at format:\n" +
            "MM meetingId\n" +
            "For example: 10 1 , where 10 is new notify time, 1 is meetingId\n" +
            "(To know meetingID u can SHOW ALL or SHOW DAY meetings)\n" +
            "\n---------------------------------------------\n";

        internal const string EnterMeetingIDToRemove = $"\n---------------------------------------------\n" +
            "\nEnter meeting id to remove\n" +
            "(To know meetingID u can SHOW ALL or SHOW DAY meetings)\n" +
            "\n---------------------------------------------\n";

        internal const string EnterDatetime = $"\n---------------------------------------------\n" +
            "\nEnter datetime to show meetings of that day\n" +
            "at format:\n" +
            "dd/MM/yyyy\n"+
            "For example: 01/01/2001\n" +
            "\n---------------------------------------------\n";

        internal const string EnterPathToFile = $"\n---------------------------------------------\n" +
            "\nEnter date and path to export meetings\n" +
            """For example: 10/10/2023 D:\\meetings.txt\""" + "\n" +
            "\n---------------------------------------------\n";

        internal const string EmptySpace = "\n\n---------------------------------------------\n";

        internal const string NoMeetings = "\n---------------------------------------------\n" +
            "No meetings";

        private static object _locker = new();
        public static void DisplayMessage(string message, MessageStatus messageStatus = MessageStatus.Default)
        {
            lock (_locker)
            {
                switch (messageStatus)
                {
                    case MessageStatus.Info:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(message);
                        break;
                    case MessageStatus.Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(message);
                        break;
                    case MessageStatus.Success:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(message);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(message);
                        break;
                }

                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
