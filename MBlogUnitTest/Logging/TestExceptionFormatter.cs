using System;
using MBlog.Logging;
using Moq;
using NUnit.Framework;

namespace MBlogUnitTest.Logging
{
    [TestFixture]
    internal class TestExceptionFormatter
    {
        [Test]
        public void GivenAMessageInformation_WhenIFormatTheMessage_ThenTheMessageFormatShouldBeCorrect()
        {
            string message = "A message";
            string path = "A path";
            string rawUrl = "A Url";
            var messageInformationMock = new Mock<IMessageInformation>();
            messageInformationMock.Setup(m => m.Path).Returns(path);
            messageInformationMock.Setup(m => m.RawUrl).Returns(rawUrl);

            var e = new Exception(message);

            string expected = "Error in Path: " + path + Environment.NewLine
                              + "Raw Url: " + rawUrl + Environment.NewLine
                              + "Message: " + message + Environment.NewLine
                              + "Source: " + e.Source + Environment.NewLine
                              + "Stack Trace: " + e.StackTrace + Environment.NewLine
                              + "Target Site: " + e.TargetSite;

            string actual = e.BuildExceptionMessage(messageInformationMock.Object);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GivenNoMessageInformation_WhenIFormatTheMessage_ThenTheMessageFormatShouldBeCorrect()
        {
            string message = "A message";
            var e = new Exception(message);

            string expected = "Message: " + message + Environment.NewLine
                              + "Source: " + e.Source + Environment.NewLine
                              + "Stack Trace: " + e.StackTrace + Environment.NewLine
                              + "Target Site: " + e.TargetSite;

            string actual = e.BuildExceptionMessage(null);
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}