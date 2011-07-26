using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using MBlog.Logging;

namespace MBlogUnitTest.Logging
{
    [TestFixture]
    class TestExceptionFormatter
    {
        [Test]
        public void GivenNoMessageInformation_WhenIFormatTheMessage_ThenTheMessageFormatShouldBeCorrect()
        {
            string message = "A message";
            Exception e = new Exception(message);
            
            string expected = "Message: " + message + Environment.NewLine 
                + "Source: " + e.Source + Environment.NewLine 
                + "Stack Trace: " + e.StackTrace + Environment.NewLine
                + "Target Site: " + e.TargetSite;

            string actual = e.BuildExceptionMessage(null);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GivenAMessageInformation_WhenIFormatTheMessage_ThenTheMessageFormatShouldBeCorrect()
        {
            string message = "A message";
            string path = "A path";
            string rawUrl = "A Url";
            Mock<IMessageInformation> messageInformationMock = new Mock<IMessageInformation>();
            messageInformationMock.Setup(m => m.Path).Returns(path);
            messageInformationMock.Setup(m => m.RawUrl).Returns(rawUrl);

            Exception e = new Exception(message);

            string expected = "Error in Path: " + path + Environment.NewLine
                + "Raw Url: " + rawUrl + Environment.NewLine
                + "Message: " + message + Environment.NewLine
                + "Source: " + e.Source + Environment.NewLine
                + "Stack Trace: " + e.StackTrace + Environment.NewLine
                + "Target Site: " + e.TargetSite;

            string actual = e.BuildExceptionMessage(messageInformationMock.Object);
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
