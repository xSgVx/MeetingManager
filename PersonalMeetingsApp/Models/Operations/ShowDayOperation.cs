using PersonalMeetingsApp.Models.Interfaces;
using PersonalMeetingsApp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalMeetingsApp.Models.Operations
{
    internal struct ShowDayOperation : IOperation
    {
        private List<IMeeting> _meetings;
        DateOnly date;

        public string InstructionMessage => Messages.EnterDatetime;
        public string SuccessMessage => string.Empty;

        public ShowDayOperation(ref List<IMeeting> meetings)
        {
            _meetings = meetings;
        }

        public void Parse(string s)
        {
            var dataArr = s.Trim().Split(' ');  //проверка массива на правильность входных данных

            if (dataArr.Length == 1)    //date
            {
                if (DateOnly.TryParse(dataArr[0], out DateOnly date))
                {
                    return;
                }
            }

            throw new Exception(Messages.DataParseError);
        }

        public void Run()
        {
            Messages.DisplayMessage(Helper.GetMeetingsString(_meetings, date));

        }
    }
}
