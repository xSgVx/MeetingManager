using PersonalMeetingsApp.Models.Interfaces;
using PersonalMeetingsApp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalMeetingsApp.Models.Operations
{
    internal struct ShowAllOperation : IOperation
    {
        private List<IMeeting> _meetings;

        public string InstructionMessage => string.Empty;
        public string SuccessMessage => string.Empty;

        public ShowAllOperation(ref List<IMeeting> meetings)
        {
            _meetings = meetings;
        }

        public void Parse(string s)
        {
            return;
        }

        public void Run()
        {
            Messages.DisplayMessage(Helper.GetMeetingsString(_meetings));
        }

    }
}
