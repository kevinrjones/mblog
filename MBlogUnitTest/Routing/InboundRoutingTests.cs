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
        public void GivenACorrectRoutesCollection_WhenIAskToEdit_ThenIGetTheEditActionForTheBlogPost()
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
        public void GivenACorrectRoutesCollection_WhenIAskForPostsIndex_ThenIGetTheAdminControllerIndexView()
        {
            TestRoute("~/Admin/Posts/index", new
            {
                controller = "Posts",
                action = "Index"
            },
            "GET");
        }

        [Test]
        public void GivenACorrectRoutesCollection_WhenIAskForCommentsIndex_ThenIGetTheAdminControllerIndexView()
        {
            TestRoute("~/Admin/Comments/index", new
            {
                controller = "Comments",
                action = "Index"
            },
            "GET");
        }

        [Test]
        public void GivenACorrectRoutesCollection_WhenIAskForDashboard_ThenIGetTheAdminControllerIndexView()
        {
            TestRoute("~/Dashboard", new
            {
                controller = "Dashboard",
                action = "Index"
            },
            "GET");
        }


        [Test]
        public void GivenACorrectRoutesCollection_WhenIAskForTheNewUserForm_ThenIGetTheEditActionForTheNewUser()
        {
            TestRoute("~/user/new", new
            {
                controller = "User",
                action = "New",
            },
            "GET");
        }

        [Test]
        public void GivenACorrectRoutesCollection_WhenIAskToRegister_ThenIGetTheEditActionForTheNewUser()
        {
            TestRoute("~/user/create", new
            {
                controller = "User",
                action = "Create",
            },
            "POST");
        }

        [Test]
        public void GivenACorrectRoutesCollection_WhenIAskForToLogin_ThenIGetTheEditActionForLogin()
        {
            TestRoute("~/session/new", new
            {
                controller = "Session",
                action = "New",
            },
            "GET");
        }

        [Test]
        public void GivenACorrectRoutesCollection_WhenILogin_ThenIGetTheEditActionForTheLogin()
        {
            TestRoute("~/session/create", new
            {
                controller = "Session",
                action = "Create",
            },
            "POST");
        }

        [Test]
        public void GivenACorrectRoutesCollection_WhenIAskForToLogout_ThenIGetTheEditActionForLogout()
        {
            TestRoute("~/session/delete", new
            {
                controller = "Session",
                action = "Delete",
            },
            "POST");
        }

        [Test]
        public void GivenACorrectRoutesCollection_WhenIAskForANewBlog_ThenIGetTheEditActionForLogin()
        {
            TestRoute("~/blog/new", new
            {
                controller = "Blog",
                action = "New",
            },
            "GET");
        }

        [Test]
        public void GivenACorrectRoutesCollection_WhenICreatANewBlog_ThenIGetTheEditActionForTheLogin()
        {
            TestRoute("~/blog/create", new
            {
                controller = "Blog",
                action = "Create"
            },
            "POST");
        }

        [Test]
        public void GivenACorrectRoutesCollection_WhenIAskToEditABlog_ThenIGetTheEditActionForLogin()
        {
            TestRoute("~/nickname/blog/edit/1", new
            {
                controller = "Blog",
                action = "Edit",
                blogId = 1,
                nickname = "nickname"
            },
            "GET");
        }

        [Test]
        public void GivenACorrectRoutesCollection_WhenIUpdateABlog_ThenIGetTheUpdateActionForBlogs()
        {
            TestRoute("~/nickname/blog/update", new
            {
                controller = "Blog",
                action = "Update",
                nickname = "nickname"
            },
            "POST");
        }

        [Test]
        public void GivenACorrectRoutesCollection_WhenIDeleteABlog_ThenIGetTheDeleteActionForBlogs()
        {
            TestRoute("~/nickname/blog/delete/1", new
            {
                controller = "Blog",
                action = "Delete",
                blogId = 1,
                nickname = "nickname"
            },
            "POST");
        }

        [Test]
        public void GivenACorrectRoutesCollection_WhenIUploadMedia_ThenIGetTheUploadActionForCreatingMedia()
        {
            TestRoute("~/nickname/media/upload", new
            {
                controller = "Media",
                action = "Upload",
                nickname = "nickname"
            },
            "POST");
        }

        [Test]
        public void GivenACorrectRoutesCollection_WhenICreateMedia_ThenIGetTheCreateActionForMedia()
        {
            TestRoute("~/nickname/media/create", new
            {
                controller = "Media",
                action = "Create",
                nickname = "nickname"
            },
            "POST");
        }

        [Test]
        public void GivenACorrectRoutesCollection_WhenIShowAMedia_ThenIGetTheShowActionForMedia()
        {
            TestRoute("~/nickname/media/2011/12/18/filename", new
            {
                controller = "Media",
                nickname = "nickname",
                action = "show",
                year = 2011,
                month = 12,
                day = 18,
                title = "filename",
            },
            "GET");
        }

        [Test]
        public void GivenACorrectRoutesCollection_WhenIAskForANewMedium_ThenIGetTheCreateActionForMedia()
        {
            TestRoute("~/nickname/media/new", new
            {
                controller = "Media",
                action = "New",
                nickname = "nickname"
            },
            "GET");
        }

        [Test]
        public void GivenACorrectRoutesCollection_WhenIAskForAListOfMedia_ThenIGetTheIndexActionForMedia()
        {
            TestRoute("~/nickname/media", new
            {
                controller = "Media",
                action = "Index",
                nickname = "nickname"
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