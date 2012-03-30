using System;
using MongoDB.Driver;
using NLog;
using NLog.Common;
using NLog.Targets;

namespace MBlogNlogService
{
    [Target("MongoDb")]
    public class MongoDbTarget : TargetWithLayout
    {
        private MongoDatabase _database;
        public string ServerUrl { get; set; }
        public string DatabaseName { get; set; }

        protected override void InitializeTarget()
        {
            MongoServer server = MongoServer.Create(ServerUrl);
            _database = server.GetDatabase(DatabaseName);
        }

        protected override void Write(LogEventInfo logEvent)
        {
            var details = new LogDetails(logEvent);

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
                InternalLogger.Error("Error when writing to database {0}", new object[] {ex});
                throw;
            }
        }

        private void WriteEventToDatabase(LogDetails details)
        {
            WriteEvent(details);
        }

        private void WriteEvent(LogDetails details)
        {
            MongoCollection<LogDetails> logEventInfos = _database.GetCollection<LogDetails>("logdetails");
            logEventInfos.Insert(details);
        }
    }
}