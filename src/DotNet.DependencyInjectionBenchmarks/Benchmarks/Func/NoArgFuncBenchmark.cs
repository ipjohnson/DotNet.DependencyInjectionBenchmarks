using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Func
{
    [BenchmarkCategory("Func")]
    public class NoArgFuncBenchmark : BaseBenchmark
    {
        public static string Description =>
            "This benchmark registers 3 small objects then resolves a no argument function for each object";

        [GlobalSetup]
        public void Setup()
        {
            var definitions = SmallObjectBenchmark.Definitions().ToArray();

            SetupContainerForTest(CreateAutofacContainer(), definitions);
            SetupContainerForTest(CreateDryIocContainer(), definitions);
            SetupContainerForTest(CreateGraceContainer(), definitions);
            SetupContainerForTest(CreateLightInjectContainer(), definitions);
        }

        #region Benchmark

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
        
        private void ExecuteBenchmark(IResolveScope scope)
        {
            scope.Resolve<Func<ISmallObjectGraphService1>>()();
            scope.Resolve<Func<ISmallObjectGraphService2>>()();
            scope.Resolve<Func<ISmallObjectGraphService3>>()();
        }
        #endregion
    }
}
