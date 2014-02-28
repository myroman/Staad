namespace Staad.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Word : DomainObject
    {
        [Required]
        public string Original { get; set; }

        [Required]
        public string Definition { get; set; }

        public string Example { get; set; }

        public int DictionaryId { get; set; }

        public Dictionary Container { get; set; }

        public Word()
        {
            Original = string.Empty;
            Definition = string.Empty;
            Example = string.Empty;
        }
    }
}