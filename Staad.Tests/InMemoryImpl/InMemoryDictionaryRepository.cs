namespace Staad.Tests.InMemoryImpl
{
    using System.Collections.Generic;
    using System.Linq;

    using Staad.Domain.Abstract;
    using Staad.Domain.Entities;

    public class InMemoryDictionaryRepository : IDictionaryRepository
    {
        private List<Dictionary> db;

        public InMemoryDictionaryRepository()
        {
            db = new List<Dictionary>();
            db.Add(new Dictionary { Id = 1, Name = "Animals" });
            db.Add(new Dictionary { Id = 2, Name = "My flat" });
        }

        public List<Dictionary> GetList()
        {
            return db;
        }

        public void Create(Dictionary dictionary)
        {
            throw new System.NotImplementedException();
        }

        public Dictionary Read(int id)
        {
            return db.FirstOrDefault(x => x.Id == id);
        }

        public void Update(Dictionary dictionary)
        {
            var foundDict = db.FirstOrDefault(x => x.Id == dictionary.Id);
            if (foundDict != null)
            {
                foundDict.Name = dictionary.Name;
                foundDict.Created = dictionary.Created;
            }
        }

        public void Delete(int id)
        {
            db.RemoveAll(x => x.Id == id);
        }

        public void Delete(Dictionary dictionary)
        {
            db.Remove(dictionary);
        }
    }
}