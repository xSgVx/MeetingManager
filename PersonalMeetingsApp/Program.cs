using NeoSmart.AsyncLock;
using PersonalMeetingsApp.Controllers;
using PersonalMeetingsApp.Utility;

object _locker = new();

var meetingManager = new MeetingManager();
meetingManager.MessagesHandler += DisplayMessage;

while (true)
{
    DisplayMessage(Messages.NextActionButton);
    var keyResponce = Console.ReadKey();

    while (true)
    {
        //add meeting
        if (keyResponce.Key == ConsoleKey.D1 ||
            keyResponce.Key == ConsoleKey.NumPad1)
        {
            DisplayMessage(Messages.EnterNewMeetingInfo);
            meetingManager.AddMeeting(Console.ReadLine());
            break;
        }

        //EDIT START TIME meeting
        if (keyResponce.Key == ConsoleKey.D2 ||
            keyResponce.Key == ConsoleKey.NumPad2)
        {
            DisplayMessage(Messages.EnterNewStartTime);
            meetingManager.EditStartTimeMeeting(Console.ReadLine());
            break;
        }

        //EDIT END TIME meeting
        if (keyResponce.Key == ConsoleKey.D3 ||
            keyResponce.Key == ConsoleKey.NumPad3)
        {
            DisplayMessage(Messages.EnterNewEndTime);
            meetingManager.EditEndTimeMeeting(Console.ReadLine());
            break;
        }

        //EDIT NOTIFICATION TIME meeting
        if (keyResponce.Key == ConsoleKey.D4 ||
            keyResponce.Key == ConsoleKey.NumPad4)
        {
            DisplayMessage(Messages.EnterNotifyTimeMeetingID);
            meetingManager.EditNotificationTimeMeeting(Console.ReadLine());
            break;
        }

        //REMOVE meeting
        if (keyResponce.Key == ConsoleKey.D5 ||
            keyResponce.Key == ConsoleKey.NumPad5)
        {
            DisplayMessage(Messages.EnterMeetingIDToRemove);
            meetingManager.RemoveMeeting(Console.ReadLine());
            break;
        }

        //SHOW ALL meetings
        if (keyResponce.Key == ConsoleKey.D6 ||
            keyResponce.Key == ConsoleKey.NumPad6)
        {
            DisplayMessage(Messages.EmptySpace);
            meetingManager.ShowAllMeetings();
            break;
        }

        //SHOW DAY meetings
        if (keyResponce.Key == ConsoleKey.D7 ||
            keyResponce.Key == ConsoleKey.NumPad7)
        {
            DisplayMessage(Messages.EnterDatetime);
            meetingManager.ShowDayMeetings(Console.ReadLine());
            break;
        }

        //EXPORT meetings
        if (keyResponce.Key == ConsoleKey.D8 ||
            keyResponce.Key == ConsoleKey.NumPad8)
        {
            DisplayMessage(Messages.EnterPathToFile);
            meetingManager.ExportMeetings(Console.ReadLine());
            break;
        }

        //close program
        if (keyResponce.Key == ConsoleKey.Escape)
        {
            return;
        }
        else
        {
            DisplayMessage(Messages.InputError);
            break;
        }
    }
}

void DisplayMessage(string message, MessageStatus messageStatus = MessageStatus.Default)
{
    lock(_locker)
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

        Console.ForegroundColor = ConsoleColor.White;
    }
}


