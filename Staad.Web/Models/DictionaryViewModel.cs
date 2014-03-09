using System.Linq;
using System.Web.Script.Serialization;

using Staad.Domain.Abstract;
using Staad.Domain.Entities;

namespace Staad.Web.Models
{
    public class DictionaryViewModel
    {
        public Dictionary Dictionary { get; private set; }

        private readonly JavaScriptSerializer javaScriptSerializer;

        private readonly IDictionaryViewSettings dictionaryViewSettings;

        public DictionaryViewModel(Dictionary dictionary, IDictionaryViewSettings dictionaryViewSettings)
        {
            Dictionary = dictionary;
            this.dictionaryViewSettings = dictionaryViewSettings;
            javaScriptSerializer = new JavaScriptSerializer();
        }

        public string GetJson()
        {
            var wordsToRender = Dictionary.Words.Take(dictionaryViewSettings.NumberOfWordsToRenderFirst).Select(x => new WordViewModel(x));
            var dictObj = new
                {
                    Dictionary.Id,
                    WordsToRenderFirst = wordsToRender,
                    NeedMoreWords = Dictionary.Words.Count > dictionaryViewSettings.NumberOfWordsToRenderFirst
                };

            return javaScriptSerializer.Serialize(dictObj);
        }
    }
}