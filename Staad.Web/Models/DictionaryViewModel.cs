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

        public string GetJson()
        {
            return javaScriptSerializer.Serialize(Dictionary);
        }
    }
}