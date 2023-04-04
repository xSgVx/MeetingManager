using PersonalMeetingsApp.Extensions;
using PersonalMeetingsApp.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PersonalMeetingsApp.Models
{
    internal class Meeting : IMeeting
    {
        public DateTime StartTime => _startTime;
        public DateTime EndTime => _endTime;
        public double NotifyMinutes => _notifyMinutes;
        public MeetingStatus MeetingStatus { get; set; } = MeetingStatus.Planed;
        public bool IsNotified { get; set; } = false;

        private DateTime _startTime;
        private DateTime _endTime;
        private double _notifyMinutes;

        public Meeting(DateTime meetingStart, double duration, double notifyMinutes)
        {
            this._startTime = meetingStart;
            this._endTime = meetingStart.AddMinutes(duration);
            this._notifyMinutes = notifyMinutes;
        }

        public Meeting(IMeeting meetingToCopy)
        {
            this._startTime = meetingToCopy.StartTime;
            this._endTime = meetingToCopy.EndTime;
            this._notifyMinutes = meetingToCopy.NotifyMinutes;
        }

        public bool TryEditStartTime(DateTime newStartTime)
        {
            if (MeetingStatus == MeetingStatus.Ended ||
                newStartTime > _endTime)
                return false;

            _startTime = newStartTime;
            return true;
        }

        public bool TryEditEndTime(DateTime newEndTime)
        {
            if (MeetingStatus == MeetingStatus.Ended ||
                newEndTime < _startTime)
                return false;

            this._endTime = newEndTime;
            return true;
        }

        public void EditNotifyTime(double notifyMinutes)
        {
            this._notifyMinutes = notifyMinutes;
        }

        public override string ToString()
        {
            return new string($"Статус встречи: {MeetingStatus.GetDescription()}{Environment.NewLine}" +
                              $"Дата встречи: {_startTime:dd:MM:yyyy},{Environment.NewLine}" +
                              $"Время встречи: с {_startTime:HH:mm} до {_endTime:HH:mm}");
        }

        public int CompareTo(IMeeting? other)
        {
            if (this?.StartTime < other?.StartTime)
                return -1;

            if (this?.StartTime > other?.StartTime)
                return 1;

            if (this?.StartTime == other?.StartTime)
                return 0;

            return 0;
        }
    }
}
