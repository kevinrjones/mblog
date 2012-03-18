using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
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
        private const string MediaFile = "../../Repositories/Media/image.png";
        private byte[] _mediaData;
        private User _user;
        private FileStream _fileStream;
        private TransactionScope _transactionScope;
        private UserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            _transactionScope = new TransactionScope();
            _mediaRepository = new MediaRepository(ConfigurationManager.ConnectionStrings["mblog"].ConnectionString);

            _fileStream = File.Open(MediaFile, FileMode.Open);

            _mediaData = new byte[_fileStream.Length];
            _fileStream.Read(_mediaData, 0, _mediaData.Length);

            _user = BuildMeA.User("email", "name", "password");

            _userRepository = new UserRepository(ConfigurationManager.ConnectionStrings["mblog"].ConnectionString);

            _userRepository.Create(_user);

            for (int i = 0; i < 3; i++)
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["mblog"].ConnectionString))
                {
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        connection.Open();
                        cmd.CommandText =
                            "INSERT INTO [Media]([title],[file_name" +
                            "], [year], [month], [day],[mime_type],[alignment],[size],[user_id],[bytes],[link_key])" +
                            "VALUES(@title, @file_name,  @year,  @month,  @day, @mime_type, @alignment, @size, @user_id, @bytes, @link_key)";
                        cmd.Parameters.AddWithValue("@title", "TestImage" + i);
                        cmd.Parameters.AddWithValue("@file_name", "file_name" + i);
                        cmd.Parameters.AddWithValue("@year", 2012);
                        cmd.Parameters.AddWithValue("@month", 12);
                        cmd.Parameters.AddWithValue("@day", 18);
                        cmd.Parameters.AddWithValue("@mime_type", "mime");
                        cmd.Parameters.AddWithValue("@alignment", 1);
                        cmd.Parameters.AddWithValue("@size", 1);
                        cmd.Parameters.AddWithValue("@user_id", _user.Id);
                        cmd.Parameters.AddWithValue("@bytes", _mediaData);
                        cmd.Parameters.AddWithValue("@link_key", "TestImage" + i);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        [Test]
        public void WhenIAddMediaToTheDatabase_ThenICanRetrieveTheFirstPage()
        {
            var retrievedMedia = _mediaRepository.GetMedia(1, 2, _user.Id);
            Assert.That(retrievedMedia.Count(), Is.EqualTo(2));
        }

        [Test]
        public void WhenIAddMediaToTheDatabase_ThenICanRetrieveTheSecondPage()
        {
            var retrievedMedia = _mediaRepository.GetMedia(2, 2, _user.Id);
            Assert.That(retrievedMedia.Count(), Is.EqualTo(1));
        }

        [Test]
        public void WhenIAddAMediumToTheDatabase_ThenICanRetrieveTheMediumByUrlAndFilename()
        {
            Media retrievedMedia = _mediaRepository.GetMedia(2012, 12, 18, "TestImage1");
            Assert.That(_mediaData, Is.EquivalentTo(retrievedMedia.Data));
        }

        [Test]
        public void WhenIAddAMediumToTheDatabase_ThenICanRetrieveTheMediumById()
        {
            Media img = _mediaRepository.WriteMedia(new Media
            {
                FileName = "file_name",
                Title = "title",
                Caption = "caption",
                Description = "description",
                Alternate = "alternate",
                UserId = _user.Id,
                MimeType = "mime",
                Alignment = (int)MBlogModel.Media.ValidAllignments.Left,
                Size = (int)MBlogModel.Media.ValidSizes.Medium,
                Data = _mediaData,
                LinkKey = "linkkey"
            });
            Media retrievedMedia = _mediaRepository.GetMedia(img.Id);
            Assert.That(_mediaData, Is.EquivalentTo(retrievedMedia.Data));
        }

        [Test]
        public void GivenAnExistingMedia_IfItIsUpdated_ThenTheMediaInTheDatabaseIsUpdated()
        {
            Media media = new Media
            {
                FileName = "filename",
                Title = "title",
                Caption = "caption",
                Description = "description",
                Alternate = "alternate",
                UserId = _user.Id,
                MimeType = "contenttype",
                Alignment = (int)Media.ValidAllignments.Left,
                Size = (int)Media.ValidSizes.Large,
                Data = new byte[] { 0, 0 },
                LinkKey = "link"
            };
            

            _mediaRepository.WriteMedia(media);

            media.FileName = "filename_new";
            _mediaRepository.UpdateMedia(media);

            var returnedMedia = _mediaRepository.Entities.Where(m => m.FileName == "filename_new").FirstOrDefault();
            Assert.That(returnedMedia, Is.Not.Null);
        }

        [TearDown]
        public void TearDown()
        {
            _transactionScope.Dispose();
            _fileStream.Close();
        }

    }
}
