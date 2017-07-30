using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Func
{
    [BenchmarkCategory("Func")]
    public class ThreeArgFuncBenchmark : StandardBenchmark
    {
        public static string Description =>
            "This benchmark registers a small object then resolves a three argument function for each object";

        protected override IEnumerable<RegistrationDefinition> Definitions
        {
            get
            {
                yield return new RegistrationDefinition
                {
                    ExportType = typeof(IThreeArgService1),
                    ActivationType = typeof(ThreeArgService1)
                };
            }
        }

        protected override void ExecuteBenchmark(IResolveScope scope)
        {
            scope.Resolve<Func<int, string, ITransientService, IThreeArgService1>>()(5, "Hello",
                new TransientService());
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
