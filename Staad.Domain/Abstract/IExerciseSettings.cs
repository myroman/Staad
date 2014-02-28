namespace Staad.Domain.Abstract
{
    using System.Collections.Generic;

    public interface IExerciseSettings
    {
        Dictionary<string, string> AbilityCssClassesMappings { get; } 
    }
}