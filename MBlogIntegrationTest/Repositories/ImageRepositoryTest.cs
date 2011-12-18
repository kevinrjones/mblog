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
        private FileStream _str;
        private TransactionScope _transactionScope;
        private UserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            _transactionScope = new TransactionScope();
            _imageRepository = new ImageRepository(ConfigurationManager.ConnectionStrings["testdb"].ConnectionString);
            
            _str = File.Open(Image, FileMode.Open);

            _imageData = new byte[_str.Length];
            _str.Read(_imageData, 0, _imageData.Length);

            _user = BuildMeA.User("email", "name", "password");

            _userRepository = new UserRepository(ConfigurationManager.ConnectionStrings["testdb"].ConnectionString);

            _userRepository.Create(_user);

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["testdb"].ConnectionString))
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    connection.Open();
                    cmd.CommandText =
                        "INSERT INTO [images]([title],[file_name], [year], [month], [day],[mime_type],[alignment],[size],[user_id],[image])" +
                                      "VALUES(@title, @file_name,  @year,  @month,  @day, @mime_type, @alignment, @size, @user_id, @image)";
                    cmd.Parameters.AddWithValue("@title", "TestImage");
                    cmd.Parameters.AddWithValue("@file_name", "file_name");
                    cmd.Parameters.AddWithValue("@year", 2012);
                    cmd.Parameters.AddWithValue("@month", 12);
                    cmd.Parameters.AddWithValue("@day", 18);
                    cmd.Parameters.AddWithValue("@mime_type", "mime");
                    cmd.Parameters.AddWithValue("@alignment", "align");
                    cmd.Parameters.AddWithValue("@size", 1);
                    cmd.Parameters.AddWithValue("@user_id", _user.Id);
                    cmd.Parameters.AddWithValue("@image", _imageData);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        [Test]
        public void WhenIAddAnImageToTheDatabase_ThenICanRetrieveTheImageByUrlAndFilename()
        {
            Image retrievedImage = _imageRepository.GetImage(2012, 12, 18, "file_name");
            Assert.That(_imageData, Is.EquivalentTo(retrievedImage.ImageData));
        }

        [Test]
        public void WhenIAddAnImageToTheDatabase_ThenICanRetrieveTheImageById()
        {
            Image img = _imageRepository.WriteImage(new Image{FileName = "file_name", Title = "title", Caption = "caption", 
                Description = "description", Alternate = "alternate", UserId = _user.Id, 
                MimeType = "mime", Alignment = "alignment", Size = (int) MBlogModel.Image.ValidSizes.Medium, ImageData = _imageData});
            Image retrievedImage = _imageRepository.GetImage(img.Id);
            Assert.That(_imageData, Is.EquivalentTo(retrievedImage.ImageData));
        }

        [TearDown]
        public void TearDown()
        {
            _transactionScope.Dispose();
            _str.Close();
        }

    }
}
