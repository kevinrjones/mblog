using TechTalk.SpecFlow;

namespace MBlogSpecs
{
    [Binding]
    public class MBlogStepDefinition
    {
        [Given(@"there are multiple blog posts")]
        public void GivenThereAreMultipleBlogPosts()
        {
            //RemoteWebDriver driver = new RemoteWebDriver(new Uri("http://localhost"), DesiredCapabilities.HtmlUnitWithJavaScript());

            //// Find the text input element by its name
            //driver.Navigate();

            //// Enter something to search for
            //IWebElement element = driver.FindElement(By.Id(""));

            // Now submit the form. WebDriver will find the form for us from the element
            //element.S
            ScenarioContext.Current.Pending();
        }

        [When(@"I navigate to the home page")]
        public void WhenINavigateToTheHomePage()
        {
            ScenarioContext.Current.Pending();
        }


        [Then(@"the posts should be listed")]
        public void ThenThePostsShouldBeListed()
        {
            ScenarioContext.Current.Pending();
        }
    }
}