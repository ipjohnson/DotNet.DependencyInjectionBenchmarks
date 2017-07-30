using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard
{
    [BenchmarkCategory("Standard")]
    public class GenericObjectBenchmark : StandardBenchmark
    {
        public static string Description =>
            @"Registers a small object graph as well as one generic service and resolves it.";
        
        protected override IEnumerable<RegistrationDefinition> Definitions
        {
            get
            {
                foreach (var registrationDefinition in SmallObjectServices.Definitions())
                {
                    yield return registrationDefinition;
                }

                yield return new RegistrationDefinition { ExportType = typeof(IGenericObjectService<>), ActivationType = typeof(GenericObjectService<>) };
            }
        }

        protected override void ExecuteBenchmark(IResolveScope scope)
        {
            scope.Resolve(typeof(IGenericObjectService<ISmallObjectService>));
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
        [BenchmarkCategory(nameof(NInject))]
        public void NInject()
        {
            ExecuteBenchmark(NInjectContainer);
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
