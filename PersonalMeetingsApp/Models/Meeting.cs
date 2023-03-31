using PersonalMeetingsApp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PersonalMeetingsApp.Models
{
    internal class Meeting : IMeeting
    {
        public DateTime StartTime => _startTime;
        public DateTime EndTime => _endTime;
        public double NotifyMinutes => _notifyMinutes;

        private DateTime _startTime;
        private DateTime _endTime;
        private double _notifyMinutes;

        public Meeting(DateTime meetingStart, double duration, double notifyMinutes)
        {
            this._startTime = meetingStart;
            this._endTime = meetingStart.AddMinutes(duration);
            this._notifyMinutes = notifyMinutes;
        }

        public Meeting(DateTime meetingStart, DateTime meetingEnd, double notifyMinutes)
        {
            this._startTime = meetingStart;
            this._endTime = meetingEnd;
            this._notifyMinutes = notifyMinutes;
        }

        public void EditStartTime(DateTime newStartTime)
        {
            if (newStartTime > _endTime)
                throw new Exception(Messages.ErrorOnEditStartTime);

            _startTime = newStartTime;
        }

        public void EditEndTime(DateTime newEndTime)
        {
            if (newEndTime < _startTime)
                throw new Exception(Messages.ErrorOnEditEndTime);

            _endTime = newEndTime;
        }

        public void EditEndTime(double durationMinutes)
        {
            if (durationMinutes < 0)
                throw new Exception(Messages.ErrorOnEditEndTime);

            _endTime = _startTime.AddMinutes(durationMinutes);
        }

        public void ChangeNotifyTime(double notifyMinutes) => _notifyMinutes = notifyMinutes;

        public override string ToString()
        {
            return new string($"Дата встречи: {_startTime.ToString("dd:MM:yyyy")},{Environment.NewLine}" +
                              $"Время встречи: с {_startTime.ToString("HH:mm")} до {_endTime.ToString("HH:mm")}");
        }
    }
}
