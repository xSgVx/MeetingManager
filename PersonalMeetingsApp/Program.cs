using PersonalMeetingsApp.Models.Interfaces;
using PersonalMeetingsApp.Models.Operations;
using PersonalMeetingsApp.Utility;

object _locker1 = new();
object _locker2 = new();

List<IMeeting> meetings = new();
Task.Run(MeetingNotifier);
Task.Run(StatusUpdater);

while (true)
{
    Messages.DisplayMessage(Messages.NextActionButton);
    var keyResponce = Console.ReadKey();

    while (true)
    {
        IOperation operation = null!;
        
        //add meeting
        if (keyResponce.Key == ConsoleKey.D1 ||
            keyResponce.Key == ConsoleKey.NumPad1)
        {
            operation = new AddOperation(ref meetings);
        }

        //EDIT START TIME meeting
        if (keyResponce.Key == ConsoleKey.D2 ||
            keyResponce.Key == ConsoleKey.NumPad2)
        {
            operation = new EditStartOperation(ref meetings);
        }

        //EDIT END TIME meeting
        if (keyResponce.Key == ConsoleKey.D3 ||
            keyResponce.Key == ConsoleKey.NumPad3)
        {
            operation = new EditEndOperation(ref meetings);
        }

        //EDIT NOTIFICATION TIME meeting
        if (keyResponce.Key == ConsoleKey.D4 ||
            keyResponce.Key == ConsoleKey.NumPad4)
        {
            operation = new EditNotificationOperation(ref meetings);
        }

        //REMOVE meeting
        if (keyResponce.Key == ConsoleKey.D5 ||
            keyResponce.Key == ConsoleKey.NumPad5)
        {
            operation = new RemoveOperation(ref meetings);
        }

        //SHOW ALL meetings
        if (keyResponce.Key == ConsoleKey.D6 ||
            keyResponce.Key == ConsoleKey.NumPad6)
        {
            operation = new ShowAllOperation(ref meetings);
        }

        //SHOW DAY meetings
        if (keyResponce.Key == ConsoleKey.D7 ||
            keyResponce.Key == ConsoleKey.NumPad7)
        {
            operation = new ShowDayOperation(ref meetings);
        }

        //EXPORT meetings
        if (keyResponce.Key == ConsoleKey.D8 ||
            keyResponce.Key == ConsoleKey.NumPad8)
        {
            operation = new ExportOperation(ref meetings);
        }

        if (operation != null)
        {
            Messages.DisplayMessage(operation.InstructionMessage);

            try
            {
                operation.Parse(Console.ReadLine());
                operation.Run();
                Messages.DisplayMessage(operation.SuccessMessage, MessageStatus.Success);

                break;
            }
            catch (Exception e)
            {
                Messages.DisplayMessage(e.Message, MessageStatus.Error);
            }

        }

        //close program
        if (keyResponce.Key == ConsoleKey.Escape)
        {
            return;
        }
        else
        {
            Messages.DisplayMessage(Messages.InputError);
            break;
        }
    }
}

Task MeetingNotifier()
{
    while (true)
    {
        if (meetings != null && meetings.Any())
        {
            var allToNotifyMeetings = meetings?.Where(m => m.IsNotified == false &&
                                                      (DateTime.Now.Day == m.StartTime.Day) &&
                                                      ((m.StartTime - DateTime.Now).Minutes < m.NotifyMinutes));

            if (allToNotifyMeetings?.Count() > 0)
            {
                foreach (var meet in allToNotifyMeetings)
                {
                    Messages.DisplayMessage(Messages.MeetingRemindInfo + Environment.NewLine +
                        meet.ToString() + Messages.EmptySpace, MessageStatus.Info);

                    meet.IsNotified = true;
                }
            }
        }

        Thread.Sleep(60000);     //1 sec
    }
}

Task StatusUpdater()
{


    while (true)
    {
        if (meetings != null && meetings.Any())
        {
            var meetingsWithOldStatus = meetings?.Where(m => (DateTime.Now > m.EndTime) &&
                                                        (m.MeetingStatus != MeetingStatus.Ended));

            if (meetingsWithOldStatus?.Count() > 0)
            {
                foreach (var meet in meetingsWithOldStatus)
                {
                    meet.MeetingStatus = MeetingStatus.Ended;
                }
            }
        }

        Thread.Sleep(60000);     //1 sec
    }
}

