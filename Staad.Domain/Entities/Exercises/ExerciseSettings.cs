namespace Staad.Domain.Entities.Exercises
{
    using System.Collections.Generic;

    public class ExerciseSettings
    {
        public ExerciseSettings()
        {
            Exercises = new List<Exercise>();
            WordsInLesson = 10;
        }

        public int WordsInLesson { get; set; }

        public List<Exercise> Exercises { get; set; }

        public bool HaveTimeLimits { get; set; }
    }
}