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
            get { return 5; }
        }

        public string GetJson()
        {
            var dictObj = new
                {
                    Dictionary.Id,
                    WordsToRenderFirst = Dictionary.Words.Take(NumberOfWordsToRenderFirst).Select(x => new WordViewModel(x))
                };

            return javaScriptSerializer.Serialize(dictObj);
        }
    }
}