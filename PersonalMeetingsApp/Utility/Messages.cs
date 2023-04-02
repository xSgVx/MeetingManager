﻿using System;
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

        internal const string AddMeetingSuccess = "Success: meeting has been added";
        internal const string EditMeetingSuccess = "Success: meeting has been changed";

        internal const string MeetingRemindInfo = "Info: next meeting soon, be ready!";
        internal const string MeetingRemovedInfo = "Info: meeting has beed removed!";

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

        internal const string EnterMeetingInfo = "\n---------------------------------------------\n" +
            "\nEnter meeting info: day, time, " +
            "meeting duration (in minutes),\n" +
            "notification time (is optional, default time is 15 minutes)\n" +
            "at format:\n" +
            "dd/MM/yyyy hh:MM MM MM or\n" +
            "dd/MM/yyyy hh:MM MM\n" +
            "For example: 01/01/2001 12:21 30 15 , where 30 minutes is duration,\n" +
            "15 minutes will be notification time" +
            "\n---------------------------------------------\n";

        internal const string EnterNewStartTime = $"\n---------------------------------------------\n" +
            "\nEnter new start time of chosen meeting\n" +
            "at format:\n" +
            "dd/MM/yyyy hh:MM meetingId\n" +
            "For example: 01/01/2001 12:21 5\n" +
            "(To know meetingID u can SHOW ALL or SHOW DAY meetings)\n" +
            "\n---------------------------------------------\n";

        internal const string EditMeetingInfo = "Enter the number of meeting thats needs to change " +
            "or press \"ESC\" to exit edit menu";


    }
}
