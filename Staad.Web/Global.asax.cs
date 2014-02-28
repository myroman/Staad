using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.Wcf;
using Staad.Domain.Impl;

namespace Staad.Web
{
    using Staad.Web.Binders;
    using Staad.Web.Models;

    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            const string DictionaryController = "Dictionary";

            routes.MapRoute(null, "dictionaries", new { controller = DictionaryController, action = "List" });
            routes.MapRoute(null, "dictionaries/new", new {controller = DictionaryController, action = "New"});
            routes.MapRoute(null, "dictionaries/{id}/edit", new {controller = DictionaryController, action = "Edit"});
            /*
             * show a dict: dictionaries/my-clothes
             * edit a dict.: dictionaries/my-clothes/edit
             * if dict.name is new: dictionaries/new-2
             */
            routes.MapRoute(
                null,
                "dictionaries/{id}",
                new
                {
                    controller = DictionaryController,
                    action = "Show"
                },
                new { id = @"\d+" });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new
                {
                    controller = DictionaryController, 
                    action = "List", 
                    id = UrlParameter.Optional
                }
            );

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