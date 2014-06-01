using System.Collections.Generic;
using System.Web.Script.Serialization;

using Staad.Domain.Entities;
using Staad.Domain.Entities.Exercises;

namespace Staad.Web.Models
{
    using System.Linq;

    public class ExerciseSetupViewModel
    {
        public List<Word> AllWords { get; private set; }

        public Lookup<Abilities, Exercise> ExerciseGroups { get; set; }

        public int LinkedDictionaryId { get; private set; }

        public ExerciseSettings Settings { get; set; }

        public ExerciseSetupViewModel()
        {
            AllWords = new List<Word>();
            Settings = new ExerciseSettings { WordsInLesson = 0 };
        }

        public ExerciseSetupViewModel(Dictionary dict)
        {
            LinkedDictionaryId = dict.Id;
            AllWords = new List<Word>(dict.Words);
            Settings = new ExerciseSettings();

            if (Settings.WordsInLesson > AllWords.Count)
            {
                Settings.WordsInLesson = AllWords.Count;
            }
        }

        public int WordsInLesson
        {
            get { return Settings.WordsInLesson; }
        }

        public string GetJson()
        {
            var viewModel = new
                {
                    WordsInLesson,
                    WordsMaxCount = AllWords.Count()
                };
            return new JavaScriptSerializer().Serialize(viewModel);
        }
    }
}