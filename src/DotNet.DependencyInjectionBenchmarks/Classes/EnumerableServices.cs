using System.Collections.Generic;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Classes
{
    public static class EnumerableServices
    {
        public static IEnumerable<RegistrationDefinition> Definitions()
        {
            yield return new RegistrationDefinition { ExportType = typeof(IEnumerableService), ActivationType = typeof(EnumerableService1), RegistrationMode = RegistrationMode.Multiple };
            yield return new RegistrationDefinition { ExportType = typeof(IEnumerableService), ActivationType = typeof(EnumerableService2), RegistrationMode = RegistrationMode.Multiple };
            yield return new RegistrationDefinition { ExportType = typeof(IEnumerableService), ActivationType = typeof(EnumerableService3), RegistrationMode = RegistrationMode.Multiple };
            yield return new RegistrationDefinition { ExportType = typeof(IEnumerableService), ActivationType = typeof(EnumerableService4), RegistrationMode = RegistrationMode.Multiple };
            yield return new RegistrationDefinition { ExportType = typeof(IEnumerableService), ActivationType = typeof(EnumerableService5), RegistrationMode = RegistrationMode.Multiple };
            yield return new RegistrationDefinition { ExportType = typeof(ISingletonService), ActivationType = typeof(SingletonService) };
            yield return new RegistrationDefinition { ExportType = typeof(ITransientService), ActivationType = typeof(TransientService) };
        }
    }

    public interface IEnumerableService
    {
        ISingletonService Singleton { get; }

        ITransientService Transient { get; }
    }

    public class EnumerableService1 : IEnumerableService
    {
        public EnumerableService1(ITransientService transient, ISingletonService singleton)
        {
            Transient = transient;
            Singleton = singleton;
        }

        public ISingletonService Singleton { get; }

        public ITransientService Transient { get; }
    }

    public class EnumerableService2 : IEnumerableService
    {
        public EnumerableService2(ITransientService transient, ISingletonService singleton)
        {
            Transient = transient;
            Singleton = singleton;
        }

        public ISingletonService Singleton { get; }

        public ITransientService Transient { get; }
    }

    public class EnumerableService3 : IEnumerableService
    {
        public EnumerableService3(ITransientService transient, ISingletonService singleton)
        {
            Transient = transient;
            Singleton = singleton;
        }

        public ISingletonService Singleton { get; }

        public ITransientService Transient { get; }
    }

    public class EnumerableService4 : IEnumerableService
    {
        public EnumerableService4(ITransientService transient, ISingletonService singleton)
        {
            Transient = transient;
            Singleton = singleton;
        }

        public ISingletonService Singleton { get; }

        public ITransientService Transient { get; }
    }

    public class EnumerableService5 : IEnumerableService
    {
        public EnumerableService5(ITransientService transient, ISingletonService singleton)
        {
            Transient = transient;
            Singleton = singleton;
        }

        public ISingletonService Singleton { get; }

        public ITransientService Transient { get; }
    }

    public interface IImportEnumerableService
    {
        IEnumerable<IEnumerableService> Services { get; }
    }

    public class ImportEnumerableService : IImportEnumerableService
    {
        public ImportEnumerableService(IEnumerable<IEnumerableService> services)
        {
            Services = services;
        }

        public IEnumerable<IEnumerableService> Services { get; }
    }
}
