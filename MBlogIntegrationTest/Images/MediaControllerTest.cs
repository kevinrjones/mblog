﻿using System;
using System.Configuration;
using System.IO;
using System.Transactions;
using System.Web.Mvc;
using Logging;
using MBlog.Controllers;
using MBlogModel;
using MBlogRepository.Interfaces;
using MBlogRepository.Repositories;
using MBlogService;
using MBlogServiceInterfaces;
using Moq;
using NUnit.Framework;

namespace MBlogIntegrationTest.Images
{
    [TestFixture]
    public class MediaControllerTest
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _transactionScope = new TransactionScope();
        }

        [TearDown]
        public void TearDown()
        {
            _transactionScope.Dispose();
        }

        #endregion

        private TransactionScope _transactionScope;

        [Test]
        public void GivenAStoredImage_WhenTheImageUrlIsConstructed_ThenTheImageIsReturned()
        {
            var logger = new Mock<ILogger>();
            string connectionString = ConfigurationManager.ConnectionStrings["mblog"].ConnectionString;
            string key = "filename";
            IUserService userService = new UserService(new UserRepository(connectionString),
                                                       new UsernameBlacklistRepository(connectionString), 
                                                       logger.Object);
            User user = userService.CreateUser("name", "email", "password");

            IMediaRepository repository = new MediaRepository(connectionString);
            IMediaService mediaService = new MediaService(repository);
            var controller = new MediaController(mediaService, logger.Object);

            var stream = new MemoryStream(new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 0});

            mediaService.WriteMedia(key, user.Id, "image/png", stream, 10);

            ActionResult result = controller.Show(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, key);
            Assert.That(result, Is.Not.Null);
        }
    }
}