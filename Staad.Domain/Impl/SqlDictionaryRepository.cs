using System;
using System.Collections.Generic;
using System.Linq;

using Staad.Domain.Abstract;
using Staad.Domain.Entities;
using Staad.Domain.Impl.Mappers;
using Staad.Domain.Scheme;

namespace Staad.Domain.Impl
{
    public class SqlDictionaryRepository : SqlRepositoryBase, IDictionaryRepository
    {
        private readonly IWordRepository wordRepository;

        public SqlDictionaryRepository(
            string connectionString,
            IWordRepository wordRepository) : base(connectionString)
        {
            this.wordRepository = wordRepository;
        }

        public List<Dictionary> GetList()
        {
            var dictionaries = DataContext.tbWordlists.Select(x => x.ToDomain()).ToList();
            foreach (var dictionary in dictionaries)
            {
                dictionary.Words.AddRange(GetRelatedWords(dictionary));
            }
            return dictionaries;
        }

        public Dictionary Read(int id)
        {
            var item = DataContext.tbWordlists.SingleOrDefault(x => x.id == id);
            if (item == null)
            {
                return null;
            }

            var dictionary = item.ToDomain();
            dictionary.Words.AddRange(GetRelatedWords(dictionary));

            return dictionary;
        }

        private IEnumerable<Word> GetRelatedWords(Dictionary dictionary)
        {
            var itsWords = wordRepository.GetList().Where(x => x.DictionaryId == dictionary.Id).ToArray();
            return itsWords;
        }

        public void Update(Dictionary dictionary)
        {
            if (dictionary.Id == 0)
            {
                Create(dictionary);
                return;
            }

            var itemToUpdate = DataContext.tbWordlists
                .SingleOrDefault(x => x.id == dictionary.Id);
            if (itemToUpdate == null)
            {
                Create(dictionary);
            }

            dictionary.FillDbObject(itemToUpdate);
            DataContext.SubmitChanges();
        }

        public void Create(Dictionary dictionary)
        {
            dictionary.Id = 0;
            dictionary.Created = DateTime.Now;

            var itemToInsert = new tbWordlist();
            dictionary.FillDbObject(itemToInsert);
            DataContext.tbWordlists
                .InsertOnSubmit(itemToInsert);
            DataContext.SubmitChanges();

            dictionary.Id = itemToInsert.id;
        }

        public void Delete(Dictionary dictionary)
        {
            var item = DataContext.tbWordlists.SingleOrDefault(x => x.id == dictionary.Id);
            if (item != null)
            {
                DeleteRelatedWords(dictionary);

                DataContext.tbWordlists.DeleteOnSubmit(item);
                DataContext.SubmitChanges();
            }
        }

        public void Delete(int id)
        {
            var dictionary = Read(id);
            Delete(dictionary);
        }

        private void DeleteRelatedWords(Dictionary dictionary)
        {
            foreach (var word in GetRelatedWords(dictionary))
            {
                wordRepository.Delete(word);
            }
        }
    }
}