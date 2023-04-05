using PersonalMeetingsApp.Models.Interfaces;
using PersonalMeetingsApp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalMeetingsApp.Models.Operations
{
    internal struct RemoveOperation : IOperation
    {
        private List<IMeeting> _meetings;
        private int id;

        public string InstructionMessage => Messages.EnterMeetingIDToRemove;
        public string SuccessMessage => Messages.RemoveMeetingSuccess;

        public RemoveOperation(ref List<IMeeting> meetings)
        {
            _meetings = meetings;
        }

        public void Parse(string s)
        {
            var dataArr = s.Trim().Split(' ');  //проверка массива на правильность входных данных

            if (dataArr.Length == 1)   //date, endTime, meetId
            {
                if (int.TryParse(dataArr[0], out id))
                {
                    return;
                }
            }

            throw new Exception(Messages.DataParseError);
        }

        public void Run()
        {
            _meetings.Remove(_meetings.ElementAt(id));
        }
    }
}
