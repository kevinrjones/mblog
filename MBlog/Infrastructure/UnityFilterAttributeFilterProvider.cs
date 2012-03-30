using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace MBlog.Infrastructure
{
    public class UnityFilterAttributeFilterProvider : FilterAttributeFilterProvider
    {
        private readonly IUnityContainer _container;

        public UnityFilterAttributeFilterProvider(IUnityContainer container)
        {
            _container = container;
        }

        protected override IEnumerable<FilterAttribute> GetControllerAttributes(
            ControllerContext controllerContext,
            ActionDescriptor actionDescriptor)
        {
            IEnumerable<FilterAttribute> attributes = base.GetControllerAttributes(controllerContext,
                                                                                   actionDescriptor);
            foreach (FilterAttribute attribute in attributes)
            {
                _container.BuildUp(attribute.GetType(), attribute);
            }

            return attributes;
        }

        protected override IEnumerable<FilterAttribute> GetActionAttributes(
            ControllerContext controllerContext,
            ActionDescriptor actionDescriptor)
        {
            IEnumerable<FilterAttribute> attributes = base.GetActionAttributes(controllerContext,
                                                                               actionDescriptor);
            foreach (FilterAttribute attribute in attributes)
            {
                _container.BuildUp(attribute.GetType(), attribute);
            }

            return attributes;
        }
    }
}