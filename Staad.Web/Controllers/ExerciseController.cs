using System.Web.Mvc;

using Staad.Domain.Abstract;
using Staad.Domain.Impl.Exercises;
using Staad.Web.Models;

namespace Staad.Web.Controllers
{
    using System.Collections.Generic;

    using Staad.Domain.Entities.Exercises;

    public class ExerciseController : Controller
    {
        private readonly IDictionaryRepository dictionaryRepository;

        private readonly IExerciseSettings exerciseSettings;

        public ExerciseController(IDictionaryRepository dictionaryRepository, IExerciseSettings exerciseSettings)
        {
            this.dictionaryRepository = dictionaryRepository;
            this.exerciseSettings = exerciseSettings;
        }

        [HttpPost]
        public ActionResult Start(ExerciseSetupViewModel exerciseSetupViewModel)
        {
            return View(exerciseSetupViewModel);
        }

        public ActionResult Setup(int dictId)
        {
            var dict = dictionaryRepository.Read(dictId);
            if (dict != null)
            {
                var model = new ExerciseSetupViewModel(dict);
                model.ExerciseGroups = ExerciseBuilder.GetAllExercises();
                ExerciseBuilder.GetDefaultList(model.Settings.Exercises); 
                foreach (var exercise in model.Settings.Exercises)
                {
                    var ability = exercise.Ability.ToString();
                    ViewData[ability] = exerciseSettings.AbilityCssClassesMappings[ability];
                }

                return View(model);
            }
            return RedirectToAction("List", "Dictionary");
        }
    }
}