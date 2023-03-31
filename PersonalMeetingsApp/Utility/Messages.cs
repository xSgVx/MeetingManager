using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalMeetingsApp.Utility
{
    internal static class Messages
    {
        public static string IntersectionError = "Error: meeting has intersection with other meetings"; 
        public static string DateParseError = "Error: input date was not correct";
        public static string AddMeetingSuccess = "Success: meeting has added";

        public static string ErrorOnEditEndTime = "Error: uncorrect time of meeting end";
        public static string ErrorOnEditStartTime = "Error: uncorrect time of meeting start";
    }
}
