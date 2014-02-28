using System.Collections.Generic;

namespace Staad.Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Dictionary : DomainObject
    {
        [Required]
        public string Name { get; set; }

        public DateTime Created { get; set; }

        public List<Word> Words { get; private set; }

        public Dictionary()
        {
            Words = new List<Word>();
        }

        public void Add(Word word)
        {
            word.Container = this;
            Words.Add(word);
        }
    }
}