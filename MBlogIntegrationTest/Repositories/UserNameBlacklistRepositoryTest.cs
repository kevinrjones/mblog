using System.Collections.Generic;
using System.Configuration;
using System.Transactions;
using MBlogIntegrationTest.Builder;
using MBlogModel;
using MBlogRepository.Repositories;
using NUnit.Framework;

namespace MBlogIntegrationTest.Repositories
{
    [TestFixture]
    internal class UserNameBlacklistRepositoryTest
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _transactionScope = new TransactionScope();

            _blackList = BuildMeA.Blacklist(_nickname);
            _blacklistRepository =
                new UsernameBlacklistRepository(ConfigurationManager.ConnectionStrings["testdb"].ConnectionString);
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
        private UsernameBlacklistRepository _blacklistRepository;
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
            Assert.That(blackList.Count, Is.EqualTo(15));
        }

        [Test]
        public void GivenABlacklist_WhenIAskForAnInValidEntry_ThenIGetAnEmptyTheEntry()
        {
            Blacklist blackList = _blacklistRepository.GetName("NameNotInList");
            Assert.That(blackList, Is.Null);
        }
    }
}