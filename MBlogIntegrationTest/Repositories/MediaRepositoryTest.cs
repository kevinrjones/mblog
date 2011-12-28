﻿using System;
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
            _mediaRepository = new MediaRepository(ConfigurationManager.ConnectionStrings["testdb"].ConnectionString);
            
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
                        "INSERT INTO [images]([title],[file_name" +
                        "], [year], [month], [day],[mime_type],[alignment],[size],[user_id],[image])" +
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
            Media retrievedMedia = _mediaRepository.GetMedia(2012, 12, 18, "file_name");
            Assert.That(_imageData, Is.EquivalentTo(retrievedMedia.ImageData));
        }

        [Test]
        public void WhenIAddAnImageToTheDatabase_ThenICanRetrieveTheImageById()
        {
            Media img = _mediaRepository.WriteMedia(new Media{FileName = "file_name", Title = "title", Caption = "caption", 
                Description = "description", Alternate = "alternate", UserId = _user.Id, 
                MimeType = "mime", Alignment = (int) MBlogModel.Media.ValidAllignments.None, Size = (int) MBlogModel.Media.ValidSizes.Medium, ImageData = _imageData});
            Media retrievedMedia = _mediaRepository.GetMedia(img.Id);
            Assert.That(_imageData, Is.EquivalentTo(retrievedMedia.ImageData));
        }

        [TearDown]
        public void TearDown()
        {
            _transactionScope.Dispose();
            _str.Close();
        }

    }
}
