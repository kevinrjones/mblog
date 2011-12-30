using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using MBlogBuilder;
using MBlogModel;
using MBlogRepository.Repositories;
using NUnit.Framework;

namespace MBlogIntegrationTest.Repositories
{
    [TestFixture]
    public class MediaRepositoryTest
    {
        private MediaRepository _mediaRepository;
        private const string Media = "../../Repositories/Media/image.png";
        private byte[] _mediaData;
        private User _user;
        private FileStream _str;
        private TransactionScope _transactionScope;
        private UserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            _transactionScope = new TransactionScope();
            _mediaRepository = new MediaRepository(ConfigurationManager.ConnectionStrings["testdb"].ConnectionString);
            
            _str = File.Open(Media, FileMode.Open);

            _mediaData = new byte[_str.Length];
            _str.Read(_mediaData, 0, _mediaData.Length);

            _user = BuildMeA.User("email", "name", "password");

            _userRepository = new UserRepository(ConfigurationManager.ConnectionStrings["testdb"].ConnectionString);

            _userRepository.Create(_user);

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["testdb"].ConnectionString))
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    connection.Open();
                    cmd.CommandText =
                        "INSERT INTO [Media]([title],[file_name" +
                        "], [year], [month], [day],[mime_type],[alignment],[size],[user_id],[medium])" +
                                      "VALUES(@title, @file_name,  @year,  @month,  @day, @mime_type, @alignment, @size, @user_id, @media)";
                    cmd.Parameters.AddWithValue("@title", "TestImage");
                    cmd.Parameters.AddWithValue("@file_name", "file_name");
                    cmd.Parameters.AddWithValue("@year", 2012);
                    cmd.Parameters.AddWithValue("@month", 12);
                    cmd.Parameters.AddWithValue("@day", 18);
                    cmd.Parameters.AddWithValue("@mime_type", "mime");
                    cmd.Parameters.AddWithValue("@alignment", 1);
                    cmd.Parameters.AddWithValue("@size", 1);
                    cmd.Parameters.AddWithValue("@user_id", _user.Id);
                    cmd.Parameters.AddWithValue("@media", _mediaData);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        [Test]
        public void WhenIAddAMediumToTheDatabase_ThenICanRetrieveTheMediumByUrlAndFilename()
        {
            Media retrievedMedia = _mediaRepository.GetMedia(2012, 12, 18, "file_name");
            Assert.That(_mediaData, Is.EquivalentTo(retrievedMedia.Data));
        }

        [Test]
        public void WhenIAddAMediumToTheDatabase_ThenICanRetrieveTheMediumById()
        {
            Media img = _mediaRepository.WriteMedia(new Media{FileName = "file_name", Title = "title", Caption = "caption", 
                Description = "description", Alternate = "alternate", UserId = _user.Id, 
                MimeType = "mime", Alignment = (int) MBlogModel.Media.ValidAllignments.None, Size = (int) MBlogModel.Media.ValidSizes.Medium, Data = _mediaData});
            Media retrievedMedia = _mediaRepository.GetMedia(img.Id);
            Assert.That(_mediaData, Is.EquivalentTo(retrievedMedia.Data));
        }

        [TearDown]
        public void TearDown()
        {
            _transactionScope.Dispose();
            _str.Close();
        }

    }
}
