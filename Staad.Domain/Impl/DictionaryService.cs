using System.Collections.Generic;

using Staad.Domain.Abstract;

namespace Staad.Domain.Impl
{
    using Staad.Domain.Entities;

    public class DictionaryService : IDictionaryService
    {
        private readonly IDictionaryRepository dictionaryRepository;

        private readonly IWordRepository wordRepository;

        public DictionaryService(
            IDictionaryRepository dictionaryRepository, 
            IWordRepository wordRepository)
        {
            this.dictionaryRepository = dictionaryRepository;
            this.wordRepository = wordRepository;
        }

        public void DeleteDictionaries(int[] dictionaryIdList)
        {
            foreach (var id in dictionaryIdList)
            {
                dictionaryRepository.Delete(id);
            }
        }

        public void DeleteWords(int[] wordIdList)
        {
            foreach (var id in wordIdList)
            {
                wordRepository.Delete(id);
            }
        }

        public void AddWordToDictionary(Word word, Dictionary dictionary)
        {
            dictionary.Add(word);

            // TODO: remove IT!
            word.DictionaryId = word.Container.Id;
            wordRepository.Update(word);
        }

        public void SaveWords(IEnumerable<Word> words)
        {
            foreach (var word in words)
            {
                wordRepository.Update(word);
            }
        }
    }
}