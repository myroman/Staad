using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Staad.Domain.Abstract;

namespace Staad.Domain.Impl
{
    public class SettingsReader : IExerciseSettings
    {
        public Dictionary<string, string> AbilityCssClassesMappings
        {
            get
            {
                var mappings = ConfigurationManager.AppSettings["AbilityCssClassesMappings"];
                return mappings.Split(';').Select(x => x.Split('|')).ToDictionary(k => k[0], v => v[1]);
            }
        }
    }
}