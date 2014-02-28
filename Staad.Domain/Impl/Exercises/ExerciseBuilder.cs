using System;
using System.Collections.Generic;
using System.Linq;

using Staad.Domain.Entities.Exercises;

namespace Staad.Domain.Impl.Exercises
{
    public class ExerciseBuilder
    {
        public static Lookup<Abilities, Exercise> GetAllExercises()
        {
            var exercises = new List<Exercise>
                {
                    new Exercise(Abilities.Reading)
                        {
                            Name = "Flash cards",
                            Description = "Look at the word and guess its description"
                        },
                    new Exercise(Abilities.Reading)
                        {
                            Name = "Reverse flash cards",
                            Description = "Look at the description and guess a corresponding word"
                        },
                    new Exercise(Abilities.Reading)
                        {
                            Name = "Choose right translation",
                            Description = "Look at the word and select right translation from the list of variants"
                        },
                    new Exercise(Abilities.Reading)
                        {
                            Name = "Choose right word",
                            Description = "Look at the translation and select right word from the list of variants"
                        },

                    new Exercise(Abilities.Writing)
                        {
                            Name = "Write word by definition",
                            Description = "Write a word corresponding to a given definition"
                        },
                    new Exercise(Abilities.Writing)
                        {
                            Name = "Write definition by word",
                            Description = "Write a definition corresponding to a given word"
                        }
                };

            return (Lookup<Abilities, Exercise>)exercises.ToLookup(x => x.Ability);
        }

        public static void GetDefaultList(List<Exercise> dest)
        {
            if (dest == null)
            {
                throw new ArgumentNullException("dest");
            }
            dest.Clear();
            dest.Add(new Exercise(Abilities.Reading)
                {
                    Name = "Flash cards",
                    Description = "Look at the word and guess its description"
                });
            dest.Add(new Exercise(Abilities.Writing)
            {
                Name = "Write word by definition",
                Description = "Write a word corresponding to a given definition"
            });
        }
    }
}