using System;
using System.ComponentModel;
using System.Configuration;
using System.Net.Sockets;
using MongoDB.Driver;
using NLog;
using NLog.Common;
using NLog.Layouts;
using NLog.Targets;

namespace MBlogNlogService
{
    [Target("MongoDb")]
    public class MongoDbTarget : TargetWithLayout
    {
        public string ServerUrl { get; set; }
        private MongoDatabase _database;
        public string DatabaseName { get; set; }

        protected override void InitializeTarget()
        {
            MongoServer server = MongoServer.Create(ServerUrl);
            _database = server.GetDatabase(DatabaseName);
        }

        protected override void Write(LogEventInfo logEvent)
        {
            LogDetails details = new LogDetails(logEvent);

            try
            {
                WriteEventToDatabase(details);
            }
            catch (Exception ex)
            {
                if (ex.MustBeRethrown())
                {
                    throw;
                }
                else
                {
                    InternalLogger.Error("Error when writing to database {0}", new object[] { ex });
                    throw;
                }
            }
        }

        private void WriteEventToDatabase(LogDetails details)
        {
            try
            {
                WriteEvent(details);
            }
            catch (Exception se)
            {
                InitializeTarget();
                WriteEvent(details);
            }
        }

        private void WriteEvent(LogDetails details)
        {
            MongoCollection<LogDetails> logEventInfos = _database.GetCollection<LogDetails>("logdetails");
            logEventInfos.Insert(details);
        }
    }
}
