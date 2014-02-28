using Staad.Domain.Entities;
using Staad.Domain.Scheme;

namespace Staad.Domain.Impl.Mappers
{
    public static class WordMapper
    {
        public static Word ToDomain(this tbWord from)
        {
            var word = new Word
            {
                Id = from.id,
                Original = from.original,
                Definition = from.definition,
                Example = from.example ?? string.Empty,
                DictionaryId = from.wordlistId
            };

            return word;
        }

        public static void FillDbItem(this Word from, tbWord to)
        {
            to.id = from.Id;
            to.original = from.Original;
            to.definition = from.Definition;
            to.example = from.Example;
            to.wordlistId = from.DictionaryId;
        }
    }
}