using System;
using System.ComponentModel;
using System.Configuration;
using NLog;
using NLog.Common;
using NLog.Layouts;
using NLog.Targets;
using Raven.Client.Document;

namespace MBlogNlogService
{
    [Target("RavenDb")]
    public class RavenDbTarget : TargetWithLayout
    {
        public string ServerUrl { get; set; }
        private DocumentStore _store;

        protected override void InitializeTarget()
        {
            _store = new DocumentStore {Url = ServerUrl};
            _store.Initialize();
        }

        protected override void Write(LogEventInfo logEvent)
        {
            try
            {
                WriteEventToDatabase(logEvent);
            }
            catch (Exception ex)
            {
                if (ex.MustBeRethrown())
                {
                    throw;
                }
                else
                {
                    InternalLogger.Error("Error when writing to database {0}", new object[]{ex});
                    throw;
                }
            }
        }

        private void WriteEventToDatabase(LogEventInfo logEvent)
        {
            using (var session = _store.OpenSession())
            {
                session.Store(logEvent);    
                session.SaveChanges();
            }
        }
    }
}
