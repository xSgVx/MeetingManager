using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalMeetingsApp.Utility
{
    internal static class ConsoleHelper
    {
        //public delegate void CancelButtonHandler();
        //public static event CancelButtonHandler? CancelEvent = Console.zCancelKeyPress;

        internal static ConsoleKeyInfo GetConsoleResponceKey()
        {
            var responce = Console.ReadKey();
            return responce;
        }

        internal static ConsoleKeyInfo GetConsoleResponceKey(string outputMessage)
        {
            DisplayMessage(outputMessage);
            var responce = Console.ReadKey();
            Console.WriteLine();

            return responce;
        }

        internal static string GetConsoleResponceString()
        {            
            var responce = Console.ReadLine();
            return responce;
        }

        //internal static string ParseConsoleInput(string input)
        //{
        //    input = input.Trim().ToLower();
        //
        //    if (input == "a")
        //    {
        //
        //    }
        //}



        internal static void DisplayMessage(string message, MessageStatus messageStatus = MessageStatus.Default)
        {
            switch (messageStatus)
            {
                case MessageStatus.Info:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(message);
                    break;
                case MessageStatus.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(message);
                    break;
                case MessageStatus.Success:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(message);
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(message);
                    break;
            }

        }

    }
}
