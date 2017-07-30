using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Collections
{
    [BenchmarkCategory("Collections")]
    public class IEnumerableBenchmark : StandardBenchmark
    {
        public static string Description =>
            "This benchmark registers 5 small objects then resolves them as an IEnumerable(T).";

        protected override IEnumerable<RegistrationDefinition> Definitions => EnumerableServices.Definitions();

        protected override void ExecuteBenchmark(IResolveScope scope)
        {
            if (scope.Resolve<IEnumerable<IEnumerableService>>().Count() != 5)
            {
                throw new Exception("Count does not equal 5");
            }
        }
        
        [Benchmark]
        [BenchmarkCategory(nameof(Autofac))]
        public void Autofac()
        {
            ExecuteBenchmark(AutofacContainer);
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
        [BenchmarkCategory(nameof(MicrosoftDependencyInjection))]
        public void MicrosoftDependencyInjection()
        {
            ExecuteBenchmark(MicrosoftDependencyInjectionContainer);
        }
        
        [Benchmark]
        [BenchmarkCategory(nameof(SimpleInjector))]
        public void SimpleInjector()
        {
            ExecuteBenchmark(SimpleInjectorContainer);
        }
        
        [Benchmark]
        [BenchmarkCategory(nameof(StructureMap))]
        public void StructureMap()
        {
            ExecuteBenchmark(StructureMapContainer);
        }
    }
}
