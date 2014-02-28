namespace Staad.Domain.Entities.Exercises
{
    public class Exercise
    {
        public Exercise(Abilities ability)
        {
            Ability = ability;
        }

        public string Name { get; set; }

        public Abilities Ability { get; private set; }

        public string Description { get; set; }
    }
}