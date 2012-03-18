using System.Collections.Generic;
using System.Configuration;
using System.Transactions;
using MBlogBuilder;
using MBlogModel;
using MBlogRepository.Repositories;
using NUnit.Framework;

namespace MBlogIntegrationTest.Repositories
{
    [TestFixture]
    internal class NicknameBlacklistRepositoryTest
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _transactionScope = new TransactionScope();

            _blackList = BuildMeA.Blacklist(_nickname);
            _blacklistRepository =
                new NicknameBlacklistRepository(ConfigurationManager.ConnectionStrings["mblog"].ConnectionString);
            _blacklistRepository.Create(_blackList);
        }

        [TearDown]
        public void TearDown()
        {
            _transactionScope.Dispose();
        }

        #endregion

        private TransactionScope _transactionScope;
        private Blacklist _blackList;
        private NicknameBlacklistRepository _blacklistRepository;
        private string _nickname = "kevin";

        [Test]
        public void GivenABlacklist_WhenIAskForAValidEntry_ThenIGetTheEntry()
        {
            Blacklist blackList = _blacklistRepository.GetName(_nickname);
            Assert.That(blackList.Name, Is.EqualTo(_nickname));
        }

        [Test]
        public void GivenABlacklist_WhenIAskForAllEntries_ThenIGetAllEntries()
        {
            List<Blacklist> blackList = _blacklistRepository.GetNames();
            Assert.That(blackList.Count, Is.EqualTo(142));
        }

        [Test]
        public void GivenABlacklist_WhenIAskForAnInValidEntry_ThenIGetAnEmptyTheEntry()
        {
            Blacklist blackList = _blacklistRepository.GetName("NameNotInList");
            Assert.That(blackList, Is.Null);
        }
    }
}