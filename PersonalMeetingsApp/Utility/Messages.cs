using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalMeetingsApp.Utility
{
    internal enum MessageStatus
    {
        Info,
        Error,
        Success,
        Default
    }

    internal static class Messages
    {
        internal const string IntersectionError = "Error: meeting has intersection with other meetings";
        internal const string DateParseError = "Error: input date was not correct";
        internal const string ErrorOnEditEndTime = "Error: uncorrect time of meeting end";
        internal const string ErrorOnEditStartTime = "Error: uncorrect time of meeting start";
        internal const string InputError = "Error: input error";
        internal const string DataParseError = "Error: error while parsing input data";
        internal const string EnteredDateError = "Error: entered date is not correct";
        internal const string MeetingsExportError = $"Error: export path not correct";

        internal const string AddMeetingSuccess = "Success: meeting has been added";
        internal const string EditMeetingSuccess = "Success: meeting time has been changed";
        internal const string EditMeetingNotifySuccess = "Success: meeting notify time has been changed";
        internal const string RemoveMeetingSuccess = "Success: meeting has been deleted";
        internal const string MeetingsExported = $"Success: meetings has been exported";

        internal const string MeetingRemindInfo = "Info: next meeting soon, be ready!";

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
            "\nEnter path to export meetings\n" +
            """For example: D:\\meetings.txt\""" + "\n" +
            "\n---------------------------------------------\n";

        internal const string EmptySpace = "\n\n---------------------------------------------\n";

        internal const string NoMeetings = "\n---------------------------------------------\n" +
            "No meetings";


    }
}
