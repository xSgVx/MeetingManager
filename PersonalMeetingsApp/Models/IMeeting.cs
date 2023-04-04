using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalMeetingsApp.Models
{
    internal interface IMeeting: IComparable<IMeeting>
    {
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public double NotifyMinutes { get; }
        public bool IsNotified { get; set; }
        public MeetingStatus MeetingStatus { get; set; }
        public void EditStartTime(DateTime newStartTime);
        public void EditEndTime(DateTime newEndTime);
        public void EditNotifyTime(double notifyMinutes);
    }
}
