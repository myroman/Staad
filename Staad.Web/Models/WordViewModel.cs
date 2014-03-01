using Staad.Domain.Entities;

namespace Staad.Web.Models
{
    public class WordViewModel
    {
        private readonly Word word;

        public WordViewModel(Word word)
        {
            this.word = word;
        }

        public int Id
        {
            get { return word.Id; }
        }

        public string Original
        {
            get { return word.Original; }
        }

        public string Definition
        {
            get { return word.Definition; }
        }

        public string Example
        {
            get { return word.Example; }
        }
    }
}