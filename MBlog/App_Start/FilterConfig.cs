using System.Web.Mvc;
using Logging;
using MBlog.Infrastructure.Logging.NLog;

namespace MBlog.App_Start
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(this GlobalFilterCollection filters)
        {
            var logger = DependencyResolver.Current.GetService<ILogger>();
            filters.Add(new NLogHandleErrorAttribute(logger));
        }
    }
}