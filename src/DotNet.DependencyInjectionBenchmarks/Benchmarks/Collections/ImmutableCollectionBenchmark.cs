using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Collections
{
    [BenchmarkCategory("Collections")]
    public class ImmutableCollectionBenchmark : StandardBenchmark
    {
        public static string Description =>
            "This benchmark registers 5 small objects then resolves them as an ImmutableList(T).";

        protected override IEnumerable<RegistrationDefinition> Definitions => EnumerableServices.Definitions();

        protected override void ExecuteBenchmark(IResolveScope scope)
        {
            if (scope.Resolve<ImmutableList<IEnumerableService>>().Count() != 5)
            {
                throw new Exception("Count does not equal 5");
            }
        }
        
        [Benchmark]
        [BenchmarkCategory(nameof(Grace))]
        public void Grace()
        {
            ExecuteBenchmark(GraceContainer);
        }   
    }
}
