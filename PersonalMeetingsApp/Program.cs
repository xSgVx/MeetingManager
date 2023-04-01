using PersonalMeetingsApp.Controllers;
using PersonalMeetingsApp.Utility;

MeetingManager meetingManager = new MeetingManager();
meetingManager.MessagesHandler += ConsoleHelper.DisplayMessage;
ConsoleKeyInfo keyResponce;
string stringResponce;



while (true)
{
    while (true)
    {
        ConsoleHelper.DisplayMessage(Messages.NextActionButton);
        keyResponce = ConsoleHelper.GetConsoleResponceKey();

        do
        {
            //add meeting
            if (keyResponce.Key == ConsoleKey.A)
            {
                ConsoleHelper.DisplayMessage(Messages.EnterMeetingInfo);
                meetingManager.AddMeeting(ConsoleHelper.GetConsoleResponceString());
                break;
            }

            //edit meeting
            if (keyResponce.Key == ConsoleKey.E)
            {
                break;
            }

            //remove meeting
            if (keyResponce.Key == ConsoleKey.R)
            {
                break;
            }

            //show meetings
            if (keyResponce.Key == ConsoleKey.S)
            {
                break;
            }

            //export meetings to file
            if (keyResponce.Key == ConsoleKey.F)
            {
                break;
            }


        } while (Console.ReadKey(true).Key != ConsoleKey.Escape ||
            Console.ReadKey(true).Key != ConsoleKey.Enter);


        //close program
        if (keyResponce.Key == ConsoleKey.Escape)
        {
            return;
        }
        else
        {
            ConsoleHelper.DisplayMessage(Messages.InputError);
        }
    }


    //meetingManager.AddMeeting("01/04/2023", "14:30", 25);
    //meetingManager.AddMeeting("12/05/2005", "11:00", 25);
    //meetingManager.AddMeeting("12/05/2024", "12:35", 25);
    //meetingManager.AddMeeting("12/05/2024", "12:40", 10);

    //MessagesToConsole(meetingManager.GetMeetingsString());
    //meetingManager.ExportMeetings("D:\\meetings.txt", true);





    //Thread.Sleep(1000);
}


