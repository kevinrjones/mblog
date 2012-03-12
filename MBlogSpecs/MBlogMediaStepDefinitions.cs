using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using MBlogBuilder;
using MBlogModel;
using MBlogRepository.Repositories;
using OpenQA.Selenium.Remote;
using TechTalk.SpecFlow;

namespace MBlogSpecs
{
    [Binding]
    public class MBlogMediaStepDefinitions
    {
        private const string Image = "../../Images/image.png";
        private byte[] _imageData;

        [BeforeScenario("image")]
        public void BeforeFeatureImage()
        {
            FileStream str = File.Open(Image, FileMode.Open);

            _imageData = new byte[str.Length];
            str.Read(_imageData, 0, _imageData.Length);

            User user = BuildMeA.User("email", "name", "password");

            UserRepository userRepository = new UserRepository(ConfigurationManager.ConnectionStrings["testdb"].ConnectionString);

            userRepository.Create(user);

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["testdb"].ConnectionString))
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    connection.Open();
                    cmd.CommandText =
                        "INSERT INTO [Media]([title],[file_name" +
                        "], [year], [month], [day],[mime_type],[alignment],[size],[user_id],[bytes])" +
                                      "VALUES(@title, @file_name,  @year,  @month,  @day, @mime_type, @alignment, @size, @user_id, @bytes)";
                    cmd.Parameters.AddWithValue("@title", "TestImage");
                    cmd.Parameters.AddWithValue("@file_name", "file_name");
                    cmd.Parameters.AddWithValue("@year", 2012);
                    cmd.Parameters.AddWithValue("@month", 12);
                    cmd.Parameters.AddWithValue("@day", 18);
                    cmd.Parameters.AddWithValue("@mime_type", "mime");
                    cmd.Parameters.AddWithValue("@alignment", 1);
                    cmd.Parameters.AddWithValue("@size", 1);
                    cmd.Parameters.AddWithValue("@user_id", user.Id);
                    cmd.Parameters.AddWithValue("@bytes", _imageData);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        [AfterScenario("image")]
        public void AfterFeatureImage()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["testdb"].ConnectionString))
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    connection.Open();
                    cmd.CommandText = "delete media; delete users; ";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        [Given(@"An image in the database")]
        public void GivenAnImageInTheDatabase()
        {
            var driver = new RemoteWebDriver(new Uri("http://localhost/mblog_test/media/2011/12/12/TestImage"), DesiredCapabilities.HtmlUnitWithJavaScript());

            // Find the text input element by its name
            driver.Navigate();

            // Enter something to search for
            // IWebElement element = driver.FindElement(By.Id(""));

            // Now submit the form. WebDriver will find the form for us from the element
            //element.S

        }

        [When(@"I view a correct image URL")]
        public void WhenIViewACorrectImageURL()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the result should be the image in the browser")]
        public void ThenTheResultShouldBeTheImageInTheBrowser()
        {
            ScenarioContext.Current.Pending();
        }

    }
}
