using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Collections
{
    [BenchmarkCategory("Collections")]
    public class ImmutableCollectionBenchmark : BaseBenchmark
    {
        public static string Description =>
            "This benchmark registers 5 small objects then resolves them as an ImmutableList(T).";

        [GlobalSetup]
        public void Setup()
        {
            var definitions = IEnumerableBenchmark.Definitions().ToArray();

            var warmup = new Action<IResolveScope>[]
            {
                scope => scope.Resolve<ImmutableList<IEnumerableService>>()
            };

            SetupContainerForTest(CreateGraceContainer(), definitions, warmup);
        }

        #region Benchmarks

        [Benchmark]
        [BenchmarkCategory("Grace")]
        public void Grace()
        {
            ExecuteBenchmark(GraceContainer);
        }
        
        private void ExecuteBenchmark(IResolveScope scope)
        {
            if (scope.Resolve<ImmutableList<IEnumerableService>>().Count() != 5)
            {
                throw new Exception("Count does not equal 5");
            }
        }

        #endregion
    }
}
