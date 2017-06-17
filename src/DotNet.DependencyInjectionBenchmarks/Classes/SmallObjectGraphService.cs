using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.DependencyInjectionBenchmarks.Classes
{
    public interface ISmallObjectGraphService1
    {
        ITransientService1 TransientService { get; }

        ISingletonService1 Singleton { get; }
    }

    public class SmallObjectGraphService1 : ISmallObjectGraphService1
    {
        public SmallObjectGraphService1(ITransientService1 transientService, ISingletonService1 singleton)
        {
            TransientService = transientService;
            Singleton = singleton;
        }

        public ITransientService1 TransientService { get; }

        public ISingletonService1 Singleton { get; }
    }

    public interface ISmallObjectGraphService2
    {
        ITransientService2 TransientService { get; }

        ISingletonService2 Singleton { get; }
    }

    public class SmallObjectGraphService2 : ISmallObjectGraphService2
    {
        public SmallObjectGraphService2(ITransientService2 transientService, ISingletonService2 singleton)
        {
            TransientService = transientService;
            Singleton = singleton;
        }

        public ITransientService2 TransientService { get; }

        public ISingletonService2 Singleton { get; }
    }

    public interface ISmallObjectGraphService3
    {
        ITransientService3 TransientService { get; }

        ISingletonService3 Singleton { get; }
    }

    public class SmallObjectGraphService3 : ISmallObjectGraphService3
    {
        public SmallObjectGraphService3(ITransientService3 transientService, ISingletonService3 singleton)
        {
            TransientService = transientService;
            Singleton = singleton;
        }

        public ITransientService3 TransientService { get; }

        public ISingletonService3 Singleton { get; }
    }
}
