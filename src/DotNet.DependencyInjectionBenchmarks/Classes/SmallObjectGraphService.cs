namespace DotNet.DependencyInjectionBenchmarks.Classes
{
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
