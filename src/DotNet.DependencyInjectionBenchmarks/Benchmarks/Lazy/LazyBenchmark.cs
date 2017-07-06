using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Lazy
{
    [BenchmarkCategory("Lazy")]
    public class LazyBenchmark : BaseBenchmark
    {
        public static string Description =>
            "This benchmark registers 3 small objects then resolves the objects using Lazy(T).";

        [GlobalSetup]
        public void Setup()
        {
            var definitions = SmallObjectBenchmark.Definitions().ToArray();

            SetupContainerForTest(CreateAutofacContainer(), definitions);
            SetupContainerForTest(CreateDryIocContainer(), definitions);
            SetupContainerForTest(CreateGraceContainer(), definitions);
            SetupContainerForTest(CreateLightInjectContainer(), definitions);
            SetupContainerForTest(CreateSimpleInjectorContainer(), definitions);
            SetupContainerForTest(CreateStructureMapContainer(), definitions);
        }

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
            ExecuteBenchmark(LightInjectContainer);
        }

        [Benchmark]
        [BenchmarkCategory("StructureMap")]
        public void StructureMap()
        {
            ExecuteBenchmark(StructureMapContainer);
        }

        private void ExecuteBenchmark(IResolveScope scope)
        {
            if (scope.Resolve<Lazy<ISmallObjectGraphService1>>().Value == null)
            {
                throw new Exception("Null lazy value");
            }

            if (scope.Resolve<Lazy<ISmallObjectGraphService2>>().Value == null)
            {
                throw new Exception("Null lazy value");
            }

            if (scope.Resolve<Lazy<ISmallObjectGraphService3>>().Value == null)
            {
                throw new Exception("Null lazy value");
            }
        }

    }
}
