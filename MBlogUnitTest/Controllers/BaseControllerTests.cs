using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MBlog.Controllers;
using MBlogUnitTest.Helpers;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Controllers
{
    public class BaseControllerTests
    {
        protected Mock<HttpContextBase> MockHttpContext;
        protected Mock<HttpRequestBase> MockRequest;
        protected FakeResponse FakeResponse;
        protected ControllerContext ControllerContext;

        [SetUp]
        public void BaseSetup()
        {
            MockHttpContext = new Mock<HttpContextBase>();
            MockRequest = new Mock<HttpRequestBase>();
            FakeResponse = new FakeResponse();

            MockHttpContext.Setup(m => m.Request).Returns(MockRequest.Object);
            MockHttpContext.Setup(m => m.Response).Returns(FakeResponse);
        }

        protected void SetControllerContext(BaseController controller)
        {
            ControllerContext = new ControllerContext(MockHttpContext.Object, new RouteData(), controller);
            controller.ControllerContext = ControllerContext;
        }

    }
}