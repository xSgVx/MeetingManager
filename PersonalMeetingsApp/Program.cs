using PersonalMeetingsApp.Controllers;

MeetingManager meetingManager = new MeetingManager();
meetingManager.MessagesHandler += MessagesToConsole;

meetingManager.AddMeeting("12/05/2005","11:00",25);
meetingManager.AddMeeting("12/05/2024","12:35",25);
meetingManager.AddMeeting("12/05/2024","12:40",10);

MessagesToConsole(meetingManager.GetMeetingsString());
meetingManager.ExportMeetings("D:\\meetings.txt", true);

void MessagesToConsole(string message)
{
    Console.WriteLine(message);
}