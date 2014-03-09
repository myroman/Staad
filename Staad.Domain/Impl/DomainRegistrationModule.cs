using System.Configuration;

using Autofac;

using Staad.Domain.Abstract;

namespace Staad.Domain.Impl
{
    public class DomainRegistrationModule : Module
    {
        private const string ConnectionStringParam = "connectionString";

        protected override void Load(ContainerBuilder builder)
        {
            // TODO: setup linq2sql connection issues.
            var connectionString = ConfigurationManager.ConnectionStrings["StaadConnString"].ConnectionString;
            
            builder.RegisterType<SqlDictionaryRepository>()
                .As<IDictionaryRepository>()
                .SingleInstance()
                .WithParameter(ConnectionStringParam, connectionString);

            builder.RegisterType<SqlWordRepository>()
                .As<IWordRepository>()
                .SingleInstance()
                .WithParameter(ConnectionStringParam, connectionString);

            builder.RegisterType<DictionaryService>()
                .As<IDictionaryService>()
                .SingleInstance();

            builder.RegisterType<SettingsReader>()
                .As<IExerciseSettings>()
                .As<IDictionaryViewSettings>()
                .SingleInstance();
        }
    }
}