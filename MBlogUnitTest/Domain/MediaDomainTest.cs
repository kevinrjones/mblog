﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MBlogDomain;
using MBlogModel;
using MBlogRepository.Interfaces;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Domain
{
    [TestFixture]
    public class MediaDomainTest
    {
        private Mock<IMediaRepository> _mediaRepository;
        private MediaDomain mediaDomain;
        private const string Filename = "foo.jpg";

        [SetUp]
        public void Setup()
        {
            _mediaRepository = new Mock<IMediaRepository>();
            mediaDomain = new MediaDomain(_mediaRepository.Object);
        }

        [Test]
        public void WhenExistingMediaIsRequested_ThenTheMediaIsReturned()
        {
            DateTime now = DateTime.Now;
            _mediaRepository.Setup(
                m => m.GetMedia(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(
                    new Media { FileName = Filename, Year = now.Year, Month = now.Month, Day = now.Day });
            var media = mediaDomain.GetMedia(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>());
            Assert.That(media, Is.Not.Null);
        }

        [Test]
        public void WhenNonExistingMediaIsRequested_ThenAnExceptionIsThrown()
        {            
            _mediaRepository.Setup(
                m => m.GetMedia(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns((Media) null);
            Assert.Throws<MBlogMediaNotFoundException>(() => mediaDomain.GetMedia(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()));            
        }

        [Test]
        public void WhenExistingMediaIsRequested_AndTheDataBaseIsNotAvailable_ThenAnMBlogExceptionIsThrown()
        {
            _mediaRepository.Setup(
                m => m.GetMedia(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Throws
                <Exception>();
            Assert.Throws<MBlogException>(() => mediaDomain.GetMedia(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()));
        }

        [Test]
        public void WhenMediaIsCreated_ThenTheUrlToTheMediaIsReturned()
        {
            DateTime now = DateTime.Now;
            _mediaRepository.Setup(
                m => m.GetMedia(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns((Media) null);
            var stream = new Mock<Stream>();

            var url = mediaDomain.WriteMedia(Filename, It.IsAny<int>(), It.IsAny<string>(), stream.Object, It.IsAny<int>());

            Assert.That(url, Is.EqualTo(string.Format("{0}/{1}/{2}/{3}", now.Year, now.Month, now.Day, Filename)));
        }

        [Test]
        public void WhenMediaIsCreated_AndTheDataBaseIsNotAvailable_ThenAnMBlogExceptionIsThrown()
        {
            _mediaRepository.Setup(m => m.GetMedia(It.IsAny<int>(),It.IsAny<int>(),It.IsAny<int>(),It.IsAny<string>())).Throws<Exception>();
            Mock<Stream> stream = new Mock<Stream>();

            Assert.Throws<MBlogException>(() => mediaDomain.WriteMedia(Filename, It.IsAny<int>(), It.IsAny<string>(), stream.Object, It.IsAny<int>()));
        }

        [Test]
        public void WhenMediaIsUpdated_ThenTheUpdatesAreWrittenToTheRepository()
        {
            DateTime now = DateTime.Now;
            _mediaRepository.Setup(
                m => m.GetMedia(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(
                    new Media { FileName = Filename, Year = now.Year, Month = now.Month, Day = now.Day });
            Mock<Stream> stream = new Mock<Stream>();

            Media media = new Media
            {
                FileName = Filename,
                Title = "title",
                Caption = "caption",
                Description = "description",
                Alternate = "alternate",
                UserId = 1,
                MimeType = "contenttype",
                Alignment = (int)Media.ValidAllignments.Left,
                Size = (int)Media.ValidSizes.Large,
                Data = new byte[] { 0, 0 }
            };
            
            mediaDomain.WriteMedia(Filename, "title", "caption", "description", "alternate", 1, "contenttype", (int)Media.ValidAllignments.Left, (int)Media.ValidSizes.Large, stream.Object, 2);

            _mediaRepository.Verify(m => m.WriteMedia(media));

        }

        [Test]
        public void WhenMediaIsUpdated_AndTheDataBaseIsNotAvailable_ThenAnMBlogExceptionIsThrown()
        {
            _mediaRepository.Setup(m => m.WriteMedia(It.IsAny<Media>())).Throws<Exception>();
            var stream = new Mock<Stream>();

            Assert.Throws<MBlogException>(() => mediaDomain.WriteMedia(Filename, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), stream.Object, It.IsAny<int>()));
        }

        [Test]
        public void GivenAnExistingMedia_WhenDuplicateMediaIsCreated_ThenAnMBlogInsertItemExceptionIsThrown()
        {
            _mediaRepository.Setup(
                m => m.GetMedia(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(
                    new Media{Id = 1});

            var stream = new Mock<Stream>();
            Assert.Throws<MBlogInsertItemException>(() => mediaDomain.WriteMedia(Filename, It.IsAny<int>(), It.IsAny<string>(), stream.Object, It.IsAny<int>()));
        }


    }
}
