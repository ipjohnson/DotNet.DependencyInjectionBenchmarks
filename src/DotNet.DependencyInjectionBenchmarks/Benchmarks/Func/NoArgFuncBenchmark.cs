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
        [GlobalSetup]
        public void Setup()
        {
            var definitions = SmallObjectBenchmark.Definitions().ToArray();

            SetupScopeForTest(CreateAutofacScope(), definitions);
            SetupScopeForTest(CreateDryIocScope(), definitions);
            SetupScopeForTest(CreateGraceScope(), definitions);
            SetupScopeForTest(CreateLightInjectScope(), definitions);
            SetupScopeForTest(CreateSimpleInjectorContainerScope(), definitions);
        }

        #region Benchmark

        [Benchmark]
        [BenchmarkCategory("Autofac")]
        public void Autofac()
        {
            ExecuteBenchmark(AutofacScope);
        }

        [Benchmark]
        [BenchmarkCategory("DryIoc")]
        public void DryIoc()
        {
            ExecuteBenchmark(DryIocScope);
        }

        [Benchmark]
        [BenchmarkCategory("Grace")]
        public void Grace()
        {
            ExecuteBenchmark(GraceScope);
        }

        [Benchmark]
        [BenchmarkCategory("LightInject")]
        public void LightInject()
        {
            ExecuteBenchmark(LightInjectScope);
        }

        [Benchmark]
        [BenchmarkCategory("SimpleInjector")]
        public void SimpleInjector()
        {
            ExecuteBenchmark(SimpleInjectorScope);
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
