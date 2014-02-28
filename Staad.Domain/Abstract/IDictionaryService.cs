using System.Collections.Generic;
using Staad.Domain.Entities;

namespace Staad.Domain.Abstract
{
    public interface IDictionaryService
    {
        void DeleteDictionaries(int[] dictionaryIdList);

        void DeleteWords(int[] wordIdList);

        void AddWordToDictionary(Word word, Dictionary dictionary);

        void SaveWords(IEnumerable<Word> words);
    }
}