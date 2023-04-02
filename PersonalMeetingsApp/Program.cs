using PersonalMeetingsApp.Controllers;
using PersonalMeetingsApp.Utility;

MeetingManager meetingManager = new MeetingManager();
meetingManager.MessagesHandler += ConsoleHelper.DisplayMessage;
ConsoleKeyInfo keyResponce;
string stringResponce;



while (true)
{
    //while (true)
    //{
    ConsoleHelper.DisplayMessage(Messages.NextActionButton);
    keyResponce = ConsoleHelper.GetConsoleResponceKey();

    do
    {
        //add meeting
        if (keyResponce.Key == ConsoleKey.D1 ||
            keyResponce.Key == ConsoleKey.NumPad1)
        {
            ConsoleHelper.DisplayMessage(Messages.EnterMeetingInfo);
            stringResponce = ConsoleHelper.GetConsoleResponceString();
            meetingManager.AddMeeting(stringResponce);
            break;
        }

        //EDIT START TIME meeting
        if (keyResponce.Key == ConsoleKey.D2 ||
            keyResponce.Key == ConsoleKey.NumPad2)
        {
            ConsoleHelper.DisplayMessage(Messages.EnterNewStartTime);
            stringResponce = ConsoleHelper.GetConsoleResponceString();
            meetingManager.EditStartTimeMeeting(stringResponce);
            break;
        }

        //EDIT END TIME meeting
        if (keyResponce.Key == ConsoleKey.D3 ||
            keyResponce.Key == ConsoleKey.NumPad3)
        {
            break;
        }

        //EDIT NOTIFICATION TIME meeting
        if (keyResponce.Key == ConsoleKey.D4 ||
            keyResponce.Key == ConsoleKey.NumPad4)
        {
            break;
        }

        //REMOVE meeting
        if (keyResponce.Key == ConsoleKey.D5 ||
            keyResponce.Key == ConsoleKey.NumPad5)
        {
            break;
        }

        //SHOW ALL meetings
        if (keyResponce.Key == ConsoleKey.D6 ||
            keyResponce.Key == ConsoleKey.NumPad6)
        {
            ConsoleHelper.DisplayMessage(meetingManager.GetMeetingsString());

            break;
        }

        //SHOW DAY meetings
        if (keyResponce.Key == ConsoleKey.D7 ||
            keyResponce.Key == ConsoleKey.NumPad7)
        {
            break;
        }

        //EXPORT meetings
        if (keyResponce.Key == ConsoleKey.D8 ||
            keyResponce.Key == ConsoleKey.NumPad8)
        {
            break;
        }


    } while (Console.ReadKey(true).Key != ConsoleKey.Escape ||
        Console.ReadKey(true).Key != ConsoleKey.Enter);

    /*
    //close program
    if (keyResponce.Key == ConsoleKey.Escape)
    {
        return;
    }
    else
    {
        ConsoleHelper.DisplayMessage(Messages.InputError);
    }
    */






    //Thread.Sleep(1000);
}


