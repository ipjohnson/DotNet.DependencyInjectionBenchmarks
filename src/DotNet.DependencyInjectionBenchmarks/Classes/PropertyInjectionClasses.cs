using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.DependencyInjectionBenchmarks.Classes
{
    public interface IPropertyInjectionService
    {
        ITransientService TransientService { get; }

        ISingletonService SingletonService { get; }
    }

    public class PropertyInjectionService : IPropertyInjectionService
    {
        public ITransientService TransientService { get; set; }

        public ISingletonService SingletonService { get; set; }
    }
}
