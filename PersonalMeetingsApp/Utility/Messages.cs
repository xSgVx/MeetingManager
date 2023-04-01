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

        internal const string AddMeetingSuccess = "Success: meeting has added";

        internal const string MeetingRemindInfo = "Info: next meeting soon, be ready!";
        internal const string MeetingRemovedInfo = "Info: meeting has beed removed!";

        internal const string NextActionButton = "\n---------------------------------------------\n" +
            "Press \"1\" to ADD new meeting,\n" +
            "\"2\" to EDIT START TIME of existing meeting,\n" +
            "\"3\" to EDIT END TIME of existing meeting,\n" +
            "\"4\" to EDIT NOTIFICATION TIME of existing meeting,\n" +
            "\"5\" to REMOVE existing meeting,\n" +
            "\"6\" to SHOW ALL meetings,\n" + 
            "\"7\" to SHOW DAY meetings,\n" +
            "\"8\" to EXPORT meetings into file\n" +
            "\"ESC\" to close program" +
            "\n---------------------------------------------\n";
        
        internal const string EnterMeetingInfo = $"\n---------------------------------------------\n" +
            $"\nEnter meeting info: day, time, " +
            "meeting duration (in minutes),\n" +
            "notification time (if change is needed, default time is 15 minutes)\n" +
            "at format:\n" +
            "dd/MM/yyyy hh:MM MM MM or\n" +
            "dd/MM/yyyy hh:MM MM\n" +
            //"where dd/MM/yyyy is date, hh:MM - start time, MM - duration (in minutes),\n" +
            //"second MM - notification time(if needed)\n" +
            "For example 01/01/2001 12:21 30 15 , where 30 minutes is duration, 15 minutes will be notification time" +
            "\n---------------------------------------------\n";

        internal const string EditMeetingInfo = "Enter the number of meeting thats needs to change " +
            "or press \"ESC\" to exit edit menu";


    }
}
