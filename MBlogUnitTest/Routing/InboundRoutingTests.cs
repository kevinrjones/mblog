using System.Web;
using MBlog;
using Moq;
using NUnit.Framework;
using System.Web.Routing;
using MBlogUnitTest.Extensions;

namespace MBlogUnitTest.Routing
{
    [TestFixture]
    public class InboundRoutingTests
    {
        [Test]
        public void GivenACorrectRoutesCollection_WhenIAskForANickname_ThenIGetTheIndexView()
        {
            TestRoute("~/nickname", new
                                     {
                                         controller = "Post",
                                         action = "Index"
                                     },
            "GET");
        }

        [Test]
        public void GivenACorrectRoutesCollection_WhenIAskForAPost_ThenIGetTheShowView()
        {
            TestRoute("~/nickname/2000/01/02/post", new
            {
                controller = "Post",
                action = "Show",
                nickname = "nickname",
                year=2000,
                month="01",
                day="02",
                link = "post"
            },
            "GET");
        }

        [Test]
        public void GivenACorrectRoutesCollection_WhenIAskForTheErrorPage_ThenIGetTheErrorInde()
        {
            TestRoute("~/Error", new
            {
                controller = "Error",
                action = "Index"
            },
            "GET");
        }

        [Test]
        public void GivenACorrectRoutesCollection_WhenIAskForSlash_ThenIGetTheHomeControllerIndexView()
        {
            TestRoute("~/", new
            {
                controller = "Home",
                action = "Index"
            },
            "GET");
        }

        [Test]
        public void GivenACorrectRoutesCollection_WhenIAskForAdmin_ThenIGetTheAdminControllerIndexView()
        {
            TestRoute("~/Admin/index", new
            {
                controller = "Admin",
                action = "Index"
            },
            "GET");
        }

        [Test]
        public void GivenACorrectRoutesCollection_WhenIAskToEdit_ThenIGetTheEditViewForTheBlogPost()
        {
            //http://localhost:7969/kevin/edit/1/25
            TestRoute("~/nickname/edit/1/25", new
            {
                nickname = "nickname",
                controller = "Post",
                action = "Edit",
                postId = "25",
                blogId = "1",
            },
            "GET");
        }


        private void TestRoute(string url, object expectedValues, string httpMethod)
        {
            RouteData routeData = url.GetRouteData(httpMethod);

            // Assert: Test the route values against expectations
            Assert.That(routeData, Is.Not.Null);
            var routeValueDictionaryExpected = new RouteValueDictionary(expectedValues);
            foreach (var expectedRouteValue in routeValueDictionaryExpected)
            {
                if (expectedRouteValue.Value == null)
                {
                    Assert.That(routeData.Values[expectedRouteValue.Key], Is.Null);
                }
                else
                {
                    Assert.That(expectedRouteValue.Value.ToString(), Is.EqualTo(
                        routeData.Values[expectedRouteValue.Key].ToString()).IgnoreCase);
                }
            }
        }

    }
}