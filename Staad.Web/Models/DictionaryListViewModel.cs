using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

using Staad.Domain.Entities;
using Staad.Domain.Helpers;

namespace Staad.Web.Models
{
    public class DictionaryListViewModel
    {
        private readonly Func<object, string> getUrlFormat;

        public IEnumerable<Dictionary> Dictionaries { get; set; }

        public DictionaryListViewModel(IEnumerable<Dictionary> dictionaries,
            Func<object, string> getUrlFormat)
        {
            this.getUrlFormat = getUrlFormat;
            Dictionaries = dictionaries;
        }

        public string GetJson()
        {
            var formattedList = Dictionaries.Select(d => new
                {
                    d.Id, 
                    d.Name, 
                    Created = d.Created.WhenItWas(), 
                    WordsCount = d.Words.Count,
                    Url = getUrlFormat(new { id=d.Id })
                }).Cast<object>().ToList();
            return new JavaScriptSerializer().Serialize(formattedList);
        }
    }
}