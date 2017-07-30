using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.DependencyInjectionBenchmarks.Classes
{
    public interface IMethodInjectionService
    {
        ITransientService TransientService { get; }

        ISingletonService SingletonService { get; }
    }

    public class MethodInjectionService : IMethodInjectionService
    {
        public ITransientService TransientService { get; private set; }

        public ISingletonService SingletonService { get; private set; }

        public void InjectionMethod(ITransientService transientService, ISingletonService singletonService)
        {
            TransientService = transientService;
            SingletonService = singletonService;
        }
    }
}
