using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Collections
{
    [BenchmarkCategory("Collections")]
    public class IReadOnlyListBenchmark : BaseBenchmark
    {
        public static string Description =>
            "This benchmark registers 5 small objects then resolves them as an IReadOnlyList(T).";

        [GlobalSetup]
        public void Setup()
        {
            var definitions = IEnumerableBenchmark.Definitions().ToArray();

            var warmup = new Action<IResolveScope>[]
            {
                scope => scope.Resolve<IReadOnlyList<IEnumerableService>>()
            };

            SetupContainerForTest(CreateAutofacContainer(), definitions, warmup);
            SetupContainerForTest(CreateDryIocContainer(), definitions, warmup);
            SetupContainerForTest(CreateGraceContainer(), definitions, warmup);
            SetupContainerForTest(CreateLightInjectContainer(), definitions, warmup);
            SetupContainerForTest(CreateSimpleInjectorContainer(), definitions, warmup);

        }

        #region Benchmarks

        [Benchmark]
        [BenchmarkCategory("Autofac")]
        public void Autofac()
        {
            ExecuteBenchmark(AutofacContainer);
        }

        [Benchmark]
        [BenchmarkCategory("DryIoc")]
        public void DryIoc()
        {
            ExecuteBenchmark(DryIocContainer);
        }

        [Benchmark]
        [BenchmarkCategory("Grace")]
        public void Grace()
        {
            ExecuteBenchmark(GraceContainer);
        }

        [Benchmark]
        [BenchmarkCategory("LightInject")]
        public void LightInject()
        {
            ExecuteBenchmark(LightInjectContainer);
        }
        
        [Benchmark]
        [BenchmarkCategory("SimpleInjector")]
        public void SimpleInjector()
        {
            ExecuteBenchmark(SimpleInjectorContainer);
        }
        
        private void ExecuteBenchmark(IResolveScope scope)
        {
            if (scope.Resolve<IReadOnlyList<IEnumerableService>>().Count() != 5)
            {
                throw new Exception("Count does not equal 5");
            }
        }

        #endregion

    }
}
