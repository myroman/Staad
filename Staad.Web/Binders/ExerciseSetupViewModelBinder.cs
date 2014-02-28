namespace Staad.Web.Binders
{
    using System.Web.Mvc;

    using Staad.Domain.Abstract;
    using Staad.Domain.Entities;
    using Staad.Domain.Entities.Exercises;
    using Staad.Web.Models;

    public class ExerciseSetupViewModelBinder : DefaultModelBinder
    {
        public const string ExerciseSetupViewModelSessionKey = "ExerciseSetupViewModel";

        private readonly IDictionaryRepository dictionaryRepository;

        public ExerciseSetupViewModelBinder()
        {
            dictionaryRepository = DependencyResolver.Current.GetService<IDictionaryRepository>();
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var form = controllerContext.HttpContext.Request.Form;

            var dictId = int.Parse(form["LinkedDictionaryId"]);
            var dict = dictionaryRepository.Read(dictId);

            var model = new ExerciseSetupViewModel(dict);
            var settings = new ExerciseSettings
                {
                    WordsInLesson = int.Parse(form["WordsInLesson"]),
                    HaveTimeLimits = bool.Parse(form["Settings.HaveTimeLimits"])
                };

            model.Settings = settings;

            return model;
        }
    }
}