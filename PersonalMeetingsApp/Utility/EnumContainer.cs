using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalMeetingsApp.Utility
{
    internal enum ParseOptions
    {
        DateTime_Duration_Notification,
        DateTime_Duration,
        DateTime_MeetingId,
        Notify_MeetingId,
        DateOnly,
        MeetingId,
        DateOnly_Path
    }

    internal enum Operation
    {
        Add,
        EditStart,
        EditEnd,
        EditNotify,
        Show,
        Remove,
        Export
    }

    internal enum MessageStatus
    {
        Info,
        Error,
        Success,
        Default
    }

    internal enum MeetingStatus
    {
        [Description("Запланировано")] Planed,
        [Description("Завершено")] Ended
    }
}
