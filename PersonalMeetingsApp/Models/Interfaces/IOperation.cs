using PersonalMeetingsApp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalMeetingsApp.Models.Interfaces
{
    internal interface IOperation
    {
        string InstructionMessage { get; }
        string SuccessMessage { get; }
        void Parse(string s);
        void Run();
    }
}
