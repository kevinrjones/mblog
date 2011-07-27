using System;
using System.Diagnostics;
using NLog;

namespace MBlogNlogService
{
    public class LogDetails
    {

        public LogDetails(LogEventInfo logEvent)
        {
            TimeStamp = logEvent.TimeStamp;
            Exception = logEvent.Exception;
            Level = logEvent.Level;
            LoggerName = logEvent.LoggerName;
            Message = logEvent.Message;
            Parameters = logEvent.Parameters;
            UserStackFrame = logEvent.UserStackFrame;
        }

        public DateTime TimeStamp { get; set; }

        public LogLevel Level { get; set; }
        public StackFrame UserStackFrame { get; set; }
        public StackTrace StackTrace { get; set; }

        public Exception Exception { get; set; }

        public string LoggerName { get; set; }
        public string Message { get; set; }

        public object[] Parameters { get; set; }
    }
}