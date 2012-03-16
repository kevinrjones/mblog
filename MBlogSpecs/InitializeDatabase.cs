using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBlogBuilder;
using MBlogModel;
using MBlogRepository.Repositories;
using TechTalk.SpecFlow;

namespace MBlogSpecs
{
    [Binding]
    public class InitializeDatabase
    {
        private const int NumberOfPosts = 12;
        static User _user;
        private const string Image = "../../Images/image.png";

        [BeforeTestRun]
        public static void SetupDataBase()
        {
            var posts = new List<Post>();

            for (int i = 0; i < NumberOfPosts; i++)
            {
                Post post = BuildMeA.Post("title " + i, "entry " + i, DateTime.Today, DateTime.Today);
                posts.Add(post);
            }


            var blog = BuildMeA.Blog("title", "description", "nickname", DateTime.Now)
                .WithPosts(posts);

            _user = BuildMeA.User("email", "name", "password")
                .WithBlog(blog);


            byte[] imageData;
            using (FileStream stream = File.Open(Image, FileMode.Open))
            {
                imageData = new byte[stream.Length];
                stream.Read(imageData, 0, imageData.Length);
            }
            Media media = BuildMeA.Media("filename", "title", "caption", "description", "alternate", "image/png", 1, 1, imageData)
                .WithUser(_user);


            var userRepository = new UserRepository(ConfigurationManager.ConnectionStrings["testdb"].ConnectionString);
            var mediaRepository = new MediaRepository(ConfigurationManager.ConnectionStrings["testdb"].ConnectionString);

            userRepository.Create(_user);

            mediaRepository.Create(media);
        }

        [AfterTestRun]
        public static void TearDownDataBase()
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["testdb"].ConnectionString))
            {
                using (var cmd = connection.CreateCommand())
                {
                    connection.Open();
                    cmd.CommandText = "delete media; delete posts; delete blogs; delete users";
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
