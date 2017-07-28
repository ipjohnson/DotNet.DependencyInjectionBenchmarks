using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.DependencyInjectionBenchmarks.Classes
{
    public class FieldInjectionService
    {
        public ITransientService TransientService;

        public ISingletonService SingletonService;
    }
}
