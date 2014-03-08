using System.Linq;
using System.Web.Script.Serialization;

using Staad.Domain.Entities;

namespace Staad.Web.Models
{
    public class DictionaryViewModel
    {
        public Dictionary Dictionary { get; private set; }

        private readonly JavaScriptSerializer javaScriptSerializer;

        public DictionaryViewModel(Dictionary dictionary)
        {
            Dictionary = dictionary;
            javaScriptSerializer = new JavaScriptSerializer();
        }

        private int NumberOfWordsToRenderFirst
        {
            get { return 20; }
        }

        public string GetJson()
        {
            var wordsToRender = Dictionary.Words.Take(NumberOfWordsToRenderFirst).Select(x => new WordViewModel(x));
            var dictObj = new
                {
                    Dictionary.Id,
                    WordsToRenderFirst = wordsToRender,
                    NeedMoreWords = Dictionary.Words.Count > NumberOfWordsToRenderFirst
                };

            return javaScriptSerializer.Serialize(dictObj);
        }
    }
}