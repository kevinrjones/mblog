using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;
using NLog;

namespace MBlogLogService
{
    public class NLogService : ILogger
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void Trace(string message, params object[] args)
        {
            Logger.Trace(message, args);
        }

        public void Debug(string message, params object[] args)
        {
            Logger.Debug(message, args);
        }

        public void Info(string message, params object[] args)
        {
            Logger.Info(message, args);
        }

        public void Warn(string message, params object[] args)
        {
            Logger.Warn(message, args);
        }

        public void Error(string message, params object[] args)
        {
            if (args.Length == 1 && args[0] is Exception)
            {
                Logger.ErrorException(message, (Exception) args[0]);
            }
            else
            {
                Logger.Error(message, args);
            }
        }

        public void Fatal(string message, params object[] args)
        {
            Logger.Fatal(message, args);
        }
    }
}
