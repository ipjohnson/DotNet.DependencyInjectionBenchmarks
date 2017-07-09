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
}
