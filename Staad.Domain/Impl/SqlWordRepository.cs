using System.Collections.Generic;
using System.Linq;

using Staad.Domain.Abstract;
using Staad.Domain.Entities;
using Staad.Domain.Impl.Mappers;
using Staad.Domain.Scheme;
namespace Staad.Domain.Impl
{
    public class SqlWordRepository : SqlRepositoryBase, IWordRepository
    {
        public SqlWordRepository(
            string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<Word> GetList()
        {
            return DataContext.tbWords.Select(x => x.ToDomain());
        }

        public void Create(Word word)
        {
            word.Id = 0;

            var itemToInsert = new tbWord();
            word.FillDbItem(itemToInsert);
            DataContext.tbWords.InsertOnSubmit(itemToInsert);
            DataContext.SubmitChanges();

            word.Id = itemToInsert.id;
        }

        public Word Read(int id)
        {
            var item = DataContext.tbWords.SingleOrDefault(x => x.id == id);
            if (item == null)
            {
                return null;
            }

            return item.ToDomain();
        }

        public void Update(Word word)
        {
            if (word.Id == 0)
            {
                Create(word);
                return;
            }

            var itemToUpdate = DataContext.tbWords.SingleOrDefault(x => x.id == word.Id);
            if (itemToUpdate == null)
            {
                Create(word);
            }

            word.FillDbItem(itemToUpdate);
            DataContext.SubmitChanges();
        }

        public void Delete(Word word)
        {
            Delete(word.Id);
        }

        public void Delete(int id)
        {
            var item = DataContext.tbWords.SingleOrDefault(x => x.id == id);
            if (item != null)
            {
                DataContext.tbWords.DeleteOnSubmit(item);
                DataContext.SubmitChanges();
            }
        }
    }
}