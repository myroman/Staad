namespace Staad.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    using NUnit.Framework;

    using Staad.Domain.Abstract;
    using Staad.Domain.Entities;
    using Staad.Domain.Impl;
    using Staad.Tests.InMemoryImpl;

    [TestFixture]
    [Category("DictionaryService")]
    public class DictionaryServiceTests
    {
        private Mock<IWordRepository> wordRepMock;

        private IDictionaryRepository dictionaryRepository;

        protected IDictionaryRepository DictionaryRepository
        {
            get
            {
                return dictionaryRepository ?? (dictionaryRepository = new InMemoryDictionaryRepository());
            }
        }

        private IWordRepository wordRepository;

        protected IWordRepository WordRepository
        {
            get
            {
                return wordRepository ?? (wordRepository = new InMemoryWordRepository());
            }
        }

        [TestFixtureSetUp]
        public void Setup()
        {
        }

        [Test]
        public void WhenPassDictIds_DeleteDictionaries()
        {
            var dictService = new DictionaryService(DictionaryRepository, WordRepository);
            dictService.DeleteDictionaries(new[] { 2 });

            var dicts = DictionaryRepository.GetList();
            Assert.AreEqual(1, dicts.Count(), "Count mismatch");
        }

        [Test]
        public void WhenPassWordIds_DeleteWords()
        {
            var dictService = new DictionaryService(DictionaryRepository, WordRepository);
            dictService.DeleteWords(new[] { 1, 3 });

            var words = WordRepository.GetList().ToArray();
            Assert.AreEqual(1, words.Count(), "Count mismatch");
            Assert.AreEqual(2, words.First().Id, "Id mismatch");
        }

    }
}