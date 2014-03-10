using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.Wcf;
using Staad.Domain.Impl;

using Staad.Web.Binders;
using Staad.Web.Models;

namespace Staad.Web
{
    public class MvcApplication : HttpApplication
    {
        internal const string DictionaryController = "Dictionary";
        internal const string ExerciseController = "Exercise";

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        /// <summary>
        /// Show a dict: dictionaries/my-clothes
        /// edit a dict.: dictionaries/my-clothes/edit
        /// if dict.name is new: dictionaries/new-2.
        /// </summary>
        /// <param name="routes">Routes to be registered.</param>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public static void RegisterRoutes(RouteCollection routes)
        {
            var idConstraints = new { id = @"\d+" };

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(null, "dictionaries", new { controller = DictionaryController, action = "List" });
            routes.MapRoute(null, "dictionaries/new", new { controller = DictionaryController, action = "New" });
            routes.MapRoute(null, "dictionaries/{id}/edit", new { controller = DictionaryController, action = "Edit" });
            routes.MapRoute(null, "dictionaries/{id}", new { controller = DictionaryController, action = "Show" }, idConstraints);

            routes.MapRoute(null, "dictionaries/{id}/setup", new { controller = ExerciseController, action = "Setup" }, idConstraints);
            routes.MapRoute(null, "dictionaries/{id}/start", new { controller = ExerciseController, action = "Start" }, idConstraints);

            var listRouteValues = new { controller = DictionaryController, action = "List", id = UrlParameter.Optional };
            routes.MapRoute("Default", "{controller}/{action}/{id}", listRouteValues);
        }

        protected void Application_Start()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterModule<DomainRegistrationModule>();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            var container = builder.Build();

            AutofacHostFactory.Container = container;
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            ModelBinders.Binders.Add(typeof(ExerciseSetupViewModel), new ExerciseSetupViewModelBinder());
        }
    }
}