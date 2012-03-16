using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using MBlogBuilder;
using MBlogModel;
using MBlogRepository.Repositories;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using TechTalk.SpecFlow;

namespace MBlogSpecs
{
    [Binding]
    public class MBlogMediaStepDefinitions
    {
        //        [BeforeScenario("image")]

        [Given(@"That I navigate to a blog post that contains an image")]
        public void GivenThatINavigateToABlogPostThatContainsAnImage()
        {
            var driver = new RemoteWebDriver(new Uri("http://localhost"), DesiredCapabilities.HtmlUnitWithJavaScript());

            // Find the text input element by its name
            driver.Navigate();

            // Enter something to search for
            IWebElement element = driver.FindElement(By.Id(""));

            // Now submit the form. WebDriver will find the form for us from the element
            //element.S
        }

        [When(@"I view the blog post")]
        public void WhenIViewTheBlogPost()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the image should be visible")]
        public void ThenTheImageShouldBeVisible()
        {
            ScenarioContext.Current.Pending();
        }

    }
}
