namespace Staad.Tests.InMemoryImpl
{
    using System.Collections.Generic;
    using System.Linq;

    using Staad.Domain.Abstract;
    using Staad.Domain.Entities;

    public class InMemoryWordRepository : IWordRepository
    {
        private readonly List<Word> db;

        public InMemoryWordRepository()
        {
            db = new List<Word>
                {
                    new Word { Id = 1, Original = "Dog",        Definition = "Home pet, considered to be the human's friend" },
                    new Word { Id = 2, Original = "Lion",       Definition = "Is considered to be the king of the fauna" },
                    new Word { Id = 3, Original = "Polar bear", Definition = "The biggest kind of the bear" }
                };
        }

        public IEnumerable<Word> GetList()
        {
            return db;
        }

        public void Create(Word word)
        {
            db.Add(word);
        }

        public Word Read(int id)
        {
            return db.FirstOrDefault(x => x.Id == id);
        }

        public void Update(Word word)
        {
            var foundWord = db.FirstOrDefault(x => x.Id == word.Id);
            if (foundWord != null)
            {
                foundWord.Original = word.Original;
                foundWord.Definition = word.Definition;
                foundWord.DictionaryId = word.DictionaryId;
            }
        }

        public void Delete(Word word)
        {
            db.RemoveAll(x => x.Id == word.Id);
        }

        public void Delete(int id)
        {
            db.RemoveAll(x => x.Id == id);
        }
    }
}