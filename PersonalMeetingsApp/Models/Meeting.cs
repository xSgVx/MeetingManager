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
    internal enum MeetingStatus
    {
        [Description("Запланировано")] Planed,
        [Description("Завершено")] Ended
    }

    internal class Meeting : IMeeting
    {
        public DateTime StartTime => _startTime;
        public DateTime EndTime => _endTime;
        public double NotifyMinutes => _notifyMinutes;
        public MeetingStatus MeetingStatus { get; set; }
        //public MeetingStatus MeetingStatus { get _status; }
        public bool IsNotified { get; set; } = false;

        private DateTime _startTime;
        private DateTime _endTime;
        private double _notifyMinutes;
        //private MeetingStatus _status;

        public Meeting(DateTime meetingStart, double duration, double notifyMinutes)
        {
            this._startTime = meetingStart;
            this._endTime = meetingStart.AddMinutes(duration);
            this._notifyMinutes = notifyMinutes;
            this.MeetingStatus = MeetingStatus.Planed;
        }

        //public Meeting(DateTime meetingStart, DateTime meetingEnd, double notifyMinutes)
        //{
        //    this._startTime = meetingStart;
        //    this._endTime = meetingEnd;
        //    this._notifyMinutes = notifyMinutes;
        //    this.MeetingStatus = MeetingStatus.Planed;
        //}

        public void EditStartTime(DateTime newStartTime)
        {
            if (newStartTime > _endTime)
                throw new Exception(Messages.ErrorOnEditStartTime);

            _startTime = newStartTime;
        }

        public void EditEndTime(double durationMinutes)
        {
            if (durationMinutes < 0)
                throw new Exception(Messages.ErrorOnEditEndTime);

            _endTime = _startTime.AddMinutes(durationMinutes);
        }

        //public void EditEndTime(DateTime newEndTime)
        //{
        //    if (newEndTime < _startTime)
        //        throw new Exception(Messages.ErrorOnEditEndTime);
        //
        //    _endTime = newEndTime;
        //}

        public void EditNotifyTime(double notifyMinutes)
        {
            _notifyMinutes = notifyMinutes;
        }

        public override string ToString()
        {
            return new string($"Дата встречи: {_startTime.ToString("dd:MM:yyyy")},{Environment.NewLine}" +
                              $"Время встречи: с {_startTime.ToString("HH:mm")} до {_endTime.ToString("HH:mm")}");
        }
    }
}
