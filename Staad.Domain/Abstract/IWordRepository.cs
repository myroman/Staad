using System.Collections.Generic;

using Staad.Domain.Entities;

namespace Staad.Domain.Abstract
{
    public interface IWordRepository
    {
        IEnumerable<Word> GetList();

        void Create(Word word);

        Word Read(int id);

        void Update(Word word);

        void Delete(Word word);

        void Delete(int id);
    }
}