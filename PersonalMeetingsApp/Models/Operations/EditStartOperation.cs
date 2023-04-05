using PersonalMeetingsApp.Models.Interfaces;
using PersonalMeetingsApp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalMeetingsApp.Models.Operations
{
    internal struct EditStartOperation : IOperation
    {
        private Meeting _meeting;
        private List<IMeeting> _meetings;
        private DateTime dateTime;
        private int id;

        public string InstructionMessage => Messages.EnterNewStartTime;
        public string SuccessMessage => Messages.EditMeetingSuccess;

        public EditStartOperation(ref List<IMeeting> meetings)
        {
            _meetings = meetings;
        }

        public void Parse(string s)
        {
            var dataArr = s.Trim().Split(' ');  //проверка массива на правильность входных данных

            if (dataArr.Length == 3)   //date, startTime or endTime, meetId
            {
                if (Helper.TryParseDate(dataArr[0] + " " + dataArr[1], out dateTime) &&
                                         int.TryParse(dataArr[2], out id))
                {
                    return;
                }
            }

            throw new Exception(Messages.DataParseError);
        }

        public void Run()
        {
            var oldMeeting = _meetings.ElementAt(id);
            Meeting editedMeeting = new(oldMeeting);

            if (!editedMeeting.TryEditStartTime(dateTime))
            {
                throw new Exception(Messages.EnteredDateError);
            }

            _meetings.Remove(oldMeeting);

            if (!Helper.HasIntersections(editedMeeting, _meetings))
            {
                _meetings.Add(editedMeeting);
            }
            else
            {
                _meetings.Add(oldMeeting);
                throw new Exception(Messages.IntersectionError);
            }
        }
    }
}
