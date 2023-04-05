using PersonalMeetingsApp.Models.Interfaces;
using PersonalMeetingsApp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalMeetingsApp.Models.Operations
{
    internal struct ExportOperation : IOperation
    {
        private List<IMeeting> _meetings;
        private DateOnly date;
        private string path = string.Empty;

        public string InstructionMessage => Messages.EnterPathToFile;
        public string SuccessMessage => Messages.MeetingsExported;

        public ExportOperation(ref List<IMeeting> meetings)
        {
            _meetings = meetings;
        }

        public void Parse(string s)
        {
            var dataArr = s.Trim().Split(' ');  //проверка массива на правильность входных данных

            if (dataArr.Length >= 2)
            {
                if (DateOnly.TryParse(dataArr[0], out DateOnly date))
                {
                    path = string.Join("", dataArr.Skip(1));
                    if (!string.IsNullOrEmpty(Path.GetDirectoryName(path)))
                    {
                        return;
                    }
                }
            }

            throw new Exception(Messages.DataParseError);
        }

        public void Run()
        {
            var meetingsString = Helper.GetMeetingsString(_meetings, date);
            if (meetingsString == Messages.NoMeetings)      //встреч для экспорта нет, создавать файл не нужно
            {
                Messages.DisplayMessage(meetingsString, MessageStatus.Info);
                return;
            }

            using (StreamWriter sw = new(path, false))    //встречи есть, пишем в файл
            {
                sw.WriteLine(meetingsString);
            }
        }

    }
}
