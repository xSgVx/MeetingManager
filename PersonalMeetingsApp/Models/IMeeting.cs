using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PersonalMeetingsApp.Models
{
    internal interface IMeeting
    {
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
    }
}
