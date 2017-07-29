using System;
using System.Collections.Immutable;
using System.Linq;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Collections
{
    [BenchmarkCategory("Collections")]
    public class ImmutableCollectionBenchmark : BaseBenchmark
    {
        #region Benchmark Definition

        public static string Description =>
            "This benchmark registers 5 small objects then resolves them as an ImmutableList(T).";

        private void ExecuteBenchmark(IResolveScope scope)
        {
            if (scope.Resolve<ImmutableList<IEnumerableService>>().Count() != 5)
            {
                throw new Exception("Count does not equal 5");
            }
        }

        #endregion

        #region Grace

        [GlobalSetup(Target = nameof(Grace))]
        public void GraceSetup()
        {
            SetupContainerForTest(CreateGraceContainer(), IEnumerableBenchmark.Definitions(), ExecuteBenchmark);
        }

        [Benchmark]
        [BenchmarkCategory(nameof(Grace))]
        public void Grace()
        {
            ExecuteBenchmark(GraceContainer);
        }
        
        #endregion
    }
}
