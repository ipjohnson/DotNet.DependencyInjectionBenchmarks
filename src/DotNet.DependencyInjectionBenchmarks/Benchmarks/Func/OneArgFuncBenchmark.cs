using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Func
{
    [BenchmarkCategory("Func")]
    public class OneArgFuncBenchmark : StandardBenchmark
    {
        public static string Description =>
            "This benchmark registers a small object then resolves a one argument function for each object";

        protected override IEnumerable<RegistrationDefinition> Definitions
        {
            get
            {
                yield return new RegistrationDefinition { ExportType = typeof(ISmallObjectService), ActivationType = typeof(SmallObjectService) };
                yield return new RegistrationDefinition { ExportType = typeof(ISingletonService), ActivationType = typeof(SingletonService), RegistrationLifestyle = RegistrationLifestyle.Singleton };
            }
        }

        protected override void ExecuteBenchmark(IResolveScope scope)
        {
            scope.Resolve<Func<ITransientService, ISmallObjectService>>()(new TransientService());
        }

        [Benchmark]
        [BenchmarkCategory(nameof(DryIoc))]
        public void DryIoc()
        {
            ExecuteBenchmark(DryIocContainer);
        }

        [Benchmark]
        [BenchmarkCategory(nameof(Grace))]
        public void Grace()
        {
            ExecuteBenchmark(GraceContainer);
        }
    }
}
