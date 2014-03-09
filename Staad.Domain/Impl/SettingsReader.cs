using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Staad.Domain.Abstract;

namespace Staad.Domain.Impl
{
    public class SettingsReader : 
        IExerciseSettings,
        IDictionaryViewSettings
    {
        public Dictionary<string, string> AbilityCssClassesMappings
        {
            get
            {
                var mappings = ConfigurationManager.AppSettings["AbilityCssClassesMappings"];
                return mappings.Split(';').Select(x => x.Split('|')).ToDictionary(k => k[0], v => v[1]);
            }
        }

        public int NumberOfWordsToRenderFirst
        {
            get { return ReadInt("NumberOfWordsToRenderFirst"); }
        }

        private int ReadInt(string key, int defaultValue = 0)
        {
            var value = ConfigurationManager.AppSettings[key];
            int intVal;
            return int.TryParse(value, out intVal) ? intVal : defaultValue;
        }
    }
}