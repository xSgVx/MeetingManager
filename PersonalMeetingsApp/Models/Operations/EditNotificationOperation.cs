using PersonalMeetingsApp.Models.Interfaces;
using PersonalMeetingsApp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalMeetingsApp.Models.Operations
{
    internal struct EditNotificationOperation : IOperation
    {
        private List<IMeeting> _meetings;
        private int id;
        private int notification;

        public string InstructionMessage => Messages.EnterNotifyTimeMeetingID;
        public string SuccessMessage => Messages.EditMeetingNotifySuccess;

        public EditNotificationOperation(ref List<IMeeting> meetings)
        {
            _meetings = meetings;
        }

        public void Parse(string s)
        {
            var dataArr = s.Trim().Split(' ');  //проверка массива на правильность входных данных

            if (dataArr.Length == 2)   //date, endTime, meetId
            {
                if (int.TryParse(dataArr[0], out notification) &&
                    int.TryParse(dataArr[1], out id))
                {
                    return;
                }
            }

            throw new Exception(Messages.DataParseError);
        }

        public void Run()
        {
            _meetings.ElementAt(id).EditNotifyTime(notification);
        }
    }
}
