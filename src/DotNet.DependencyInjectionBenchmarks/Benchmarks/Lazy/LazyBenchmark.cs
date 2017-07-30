using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Lazy
{
    [BenchmarkCategory("Lazy")]
    public class LazyBenchmark : StandardBenchmark
    {
        public static string Description =>
            "This benchmark registers a small object then resolves the object using Lazy(T).";

        protected override IEnumerable<RegistrationDefinition> Definitions => SmallObjectServices.Definitions();

        protected override void ExecuteBenchmark(IResolveScope scope)
        {
            if (scope.Resolve<Lazy<ISmallObjectService>>().Value == null)
            {
                throw new Exception("Null lazy value");
            }
        }

        [Benchmark]
        [BenchmarkCategory(nameof(Autofac))]
        public void Autofac()
        {
            ExecuteBenchmark(AutofacContainer);
        }
        
        [Benchmark]
        [BenchmarkCategory(nameof(CastleWindsor))]
        public void CastleWindsor()
        {
            ExecuteBenchmark(CastleWindsorContainer);
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

        [Benchmark]
        [BenchmarkCategory(nameof(LightInject))]
        public void LightInject()
        {
            ExecuteBenchmark(LightInjectContainer);
        }

        [Benchmark]
        [BenchmarkCategory(nameof(SimpleInjector))]
        public void SimpleInjector()
        {
            ExecuteBenchmark(LightInjectContainer);
        }

        [Benchmark]
        [BenchmarkCategory(nameof(StructureMap))]
        public void StructureMap()
        {
            ExecuteBenchmark(StructureMapContainer);
        }
    }
}
