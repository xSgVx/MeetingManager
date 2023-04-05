using PersonalMeetingsApp.Models.Interfaces;
using PersonalMeetingsApp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalMeetingsApp.Models.Operations
{
    internal struct AddOperation : IOperation
    {
        private Meeting _meeting;
        private List<IMeeting> _meetings;
        private const int defaultNotification = 15;

        public string InstructionMessage => Messages.EnterNewMeetingInfo;
        public string SuccessMessage => Messages.AddMeetingSuccess;

        public AddOperation(ref List<IMeeting> meetings)
        {
            _meetings = meetings;
        }

        public void Parse(string s)
        {
            var dataArr = s.Trim().Split(' ');  //проверка массива на правильность входных данных

            DateTime dateTime;
            int duration, notification;

            if (dataArr.Length == 4)   //date, startTime, duration, notification
            {
                if (Helper.TryParseDate(dataArr[0] + " " + dataArr[1], out dateTime) &&
                            int.TryParse(dataArr[2], out duration) &&
                            int.TryParse(dataArr[3], out notification))
                {
                    _meeting = new Meeting(dateTime, duration, notification);
                    return;
                }
            }

            if (dataArr.Length == 3)   //date, startTime, duration
            {
                if (Helper.TryParseDate(dataArr[0] + " " + dataArr[1], out dateTime) &&
                            int.TryParse(dataArr[2], out duration))
                {
                    _meeting = new Meeting(dateTime, duration, defaultNotification);
                    return;
                }
            }

            throw new Exception(Messages.DataParseError);
        }

        public void Run()
        {
            if (!Helper.HasIntersections(_meeting, _meetings))
            {
                _meetings.Add(_meeting);
            }
            else
            {
                throw new Exception(Messages.IntersectionError);
            }
        }


    }
}
