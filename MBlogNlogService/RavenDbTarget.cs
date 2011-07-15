using System;
using System.ComponentModel;
using System.Configuration;
using NLog;
using NLog.Common;
using NLog.Layouts;
using NLog.Targets;

namespace MBlogNlogService
{
    [Target("RavenDb")]
    public class RavenDbTarget : TargetWithLayout
    {
        public Layout ConnectionString { get; set; }
        public string ConnectionStringName { get; set; }
        private ConnectionStringSettingsCollection ConnectionStringsSettings { get; set; }

        [DefaultValue(true)]
        public bool KeepConnection { get; set; }

        public RavenDbTarget()
        {
            ConnectionStringsSettings = ConfigurationManager.ConnectionStrings;    
        }

        protected override void InitializeTarget()
        {
            bool flag = false;
            if (!string.IsNullOrEmpty(ConnectionStringName))
            {
                ConnectionStringSettings connectionStringSettings = ConnectionStringsSettings[ConnectionStringName];
                if (connectionStringSettings == null)
                    throw new NLogConfigurationException("Connection string '" + ConnectionStringName + "' is not declared in <connectionStrings /> section.");
                ConnectionString = (Layout)SimpleLayout.Escape(connectionStringSettings.ConnectionString);
                flag = true;
            }        
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
                    CloseConnection();
                    throw;
                }
            }
            finally
            {
                if (!KeepConnection)
                {
                    CloseConnection();
                }
            }
        }

        private void WriteEventToDatabase(LogEventInfo logEvent)
        {
        }

        protected override void CloseTarget()
        {
            base.CloseTarget();
            CloseConnection();
        }

        private void CloseConnection()
        {
        }

    }
}
