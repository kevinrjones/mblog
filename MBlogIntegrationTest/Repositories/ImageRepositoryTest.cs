using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using MBlogIntegrationTest.Builder;
using MBlogModel;
using MBlogRepository.Repositories;
using NUnit.Framework;

namespace MBlogIntegrationTest.Repositories
{
    [TestFixture]
    public class ImageRepositoryTest
    {
        private ImageRepository _imageRepository;
        private const string Image = "../../Repositories/Images/image.png";
        private byte[] _imageData;
        private User _user;

        private TransactionScope _transactionScope;
        private UserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            _transactionScope = new TransactionScope();
            _imageRepository = new ImageRepository(ConfigurationManager.ConnectionStrings["testdb"].ConnectionString);
            FileStream str = File.Open(Image, FileMode.Open);

            _imageData = new byte[str.Length];
            str.Read(_imageData, 0, _imageData.Length);

            _user = BuildMeA.User("email", "name", "password");

            _userRepository = new UserRepository(ConfigurationManager.ConnectionStrings["testdb"].ConnectionString);

            _userRepository.Create(_user);

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["testdb"].ConnectionString))
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    connection.Open();
                    cmd.CommandText =
                        "INSERT INTO [images]([name],[mime_type],[width],[user_id],[image])VALUES(@name, @mimeType, @width, @userid, @image)";
                    cmd.Parameters.AddWithValue("@name", "TestImage");
                    cmd.Parameters.AddWithValue("@mimetype", "mime");
                    cmd.Parameters.AddWithValue("@width", "width");
                    cmd.Parameters.AddWithValue("@userid", _user.Id);
                    cmd.Parameters.AddWithValue("@image", _imageData);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        [Test]
        public void WhenIAddAnImageToTheDataBase_ThenICanRetrieveTheImageByName()
        {
            Image retrievedImage = _imageRepository.GetImage("urlPrefix", "filename");
            Assert.That(_imageData, Is.EquivalentTo(retrievedImage.ImageData));
        }

        [TearDown]
        public void TearDown()
        {
            _transactionScope.Dispose();
        }

    }
}
