using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Scoped
{
    [BenchmarkCategory("Scoped")]
    public class CreateScopeAndResolveBenchmark : BaseBenchmark
    {
        [GlobalSetup]
        public void Setup()
        {
            var definitions = SmallObjectBenchmark.Definitions().ToArray();

            SetupScopeForTest(CreateAutofacScope(), definitions);
            SetupScopeForTest(CreateDryIocScope(), definitions);
            SetupScopeForTest(CreateGraceScope(), definitions);
            SetupScopeForTest(CreateLightInjectScope(), definitions);
            SetupScopeForTest(CreateStructureMapContainer(), definitions);
        }

        #region Benchmarks

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
        [BenchmarkCategory("StructureMap")]
        public void StructureMap()
        {
            ExecuteBenchmark(StructureMapContainer);
        }

        private void ExecuteBenchmark(IResolveScope scope)
        {
            using (var childScope = scope.CreateScope())
            {
                childScope.Resolve(typeof(ISmallObjectGraphService1));
                childScope.Resolve(typeof(ISmallObjectGraphService2));
                childScope.Resolve(typeof(ISmallObjectGraphService3));
            }
        }

        #endregion
    }
}
