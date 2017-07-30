using System.Collections.Generic;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Classes
{
    public static class SmallObjectServices
    {
        public static IEnumerable<RegistrationDefinition> Definitions(RegistrationLifestyle lifestyle = RegistrationLifestyle.Transient, object lifestyleInfo = null)
        {
            yield return new RegistrationDefinition { ExportType = typeof(ISingletonService), ActivationType = typeof(SingletonService), RegistrationLifestyle = RegistrationLifestyle.Singleton };
            yield return new RegistrationDefinition { ExportType = typeof(ISmallObjectService), ActivationType = typeof(SmallObjectService), RegistrationLifestyle = lifestyle, LifestyleInformation = lifestyleInfo };
            yield return new RegistrationDefinition { ExportType = typeof(ITransientService), ActivationType = typeof(TransientService) };
        }
    }

    public interface ISmallObjectService
    {
        ITransientService TransientService { get; }

        ISingletonService Singleton { get; }
    }

    public class SmallObjectService : ISmallObjectService
    {
        public SmallObjectService(ITransientService transientService, ISingletonService singleton)
        {
            TransientService = transientService;
            Singleton = singleton;
        }

        public ITransientService TransientService { get; }

        public ISingletonService Singleton { get; }
    }
}
