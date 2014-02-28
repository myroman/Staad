using Staad.Domain.Entities;
using Staad.Domain.Scheme;

namespace Staad.Domain.Impl.Mappers
{
    
    public static class DictionaryMapper
    {
        public static Dictionary ToDomain(this tbWordlist item)
        {
            return new Dictionary
            {
                Id = item.id,
                Name = item.name,
                Created = item.created
            };
        }

        public static void FillDbObject(this Dictionary from, tbWordlist to)
        {
            to.id = from.Id;
            to.name = from.Name;
            to.created = from.Created;
        }
    }
}