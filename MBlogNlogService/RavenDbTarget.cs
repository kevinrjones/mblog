using System;
using NLog;
using NLog.Common;
using NLog.Targets;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Extensions;

namespace MBlogNlogService
{
    [Target("RavenDb")]
    public class RavenDbTarget : TargetWithLayout
    {
        private DocumentStore _store;
        public string ServerUrl { get; set; }
        public string DatabaseName { get; set; }

        protected override void InitializeTarget()
        {
            _store = new DocumentStore {Url = ServerUrl};
            _store.Initialize();
            _store.DatabaseCommands.EnsureDatabaseExists(DatabaseName);
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
            using (IDocumentSession session = _store.OpenSession(DatabaseName))
            {
                session.Store(details);
                session.SaveChanges();
            }
        }
    }
}