using System.Collections.Generic;
using System.Linq;

using Staad.Domain.Entities;

namespace Staad.Domain.Abstract
{
    public interface IDictionaryRepository
    {
        List<Dictionary> GetList();
        
        void Create(Dictionary dictionary);

        Dictionary Read(int id); 
        
        void Update(Dictionary dictionary);

        void Delete(int id);

        void Delete(Dictionary dictionary);
    }
}