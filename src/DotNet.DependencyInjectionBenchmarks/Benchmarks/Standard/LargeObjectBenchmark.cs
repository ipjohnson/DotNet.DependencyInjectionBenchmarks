using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard
{
    [BenchmarkCategory("Standard")]
    public class LargeObjectBenchmark : StandardBenchmark
    {
        public static string Description =>
            @"Resolves a Large object graph containing 7 transients and 3 Singletons from each container";

        protected override IEnumerable<RegistrationDefinition> Definitions
        {
            get
            {
                yield return new RegistrationDefinition { ExportType = typeof(ILargeSingletonService1), ActivationType = typeof(LargeSingletonService1), RegistrationLifestyle = RegistrationLifestyle.Singleton };
                yield return new RegistrationDefinition { ExportType = typeof(ILargeSingletonService2), ActivationType = typeof(LargeSingletonService2), RegistrationLifestyle = RegistrationLifestyle.Singleton };
                yield return new RegistrationDefinition { ExportType = typeof(ILargeSingletonService3), ActivationType = typeof(LargeSingletonService3), RegistrationLifestyle = RegistrationLifestyle.Singleton };

                yield return new RegistrationDefinition { ExportType = typeof(ILargeComplexService), ActivationType = typeof(LargeComplexService) };

                yield return new RegistrationDefinition { ExportType = typeof(ILargeTransient1), ActivationType = typeof(LargeTransient1) };
                yield return new RegistrationDefinition { ExportType = typeof(ILargeTransient2), ActivationType = typeof(LargeTransient2) };
                yield return new RegistrationDefinition { ExportType = typeof(ILargeTransient3), ActivationType = typeof(LargeTransient3) };

                yield return new RegistrationDefinition { ExportType = typeof(ILargeObjectService1), ActivationType = typeof(LargeObjectService1) };
                yield return new RegistrationDefinition { ExportType = typeof(ILargeObjectService2), ActivationType = typeof(LargeObjectService2) };
                yield return new RegistrationDefinition { ExportType = typeof(ILargeObjectService3), ActivationType = typeof(LargeObjectService3) };
            }
        }

        protected override void ExecuteBenchmark(IResolveScope scope)
        {
            scope.Resolve(typeof(ILargeComplexService));
        }
        
        #region Benchmarks

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
        [BenchmarkCategory(nameof(MicrosoftDependencyInjection))]
        public void MicrosoftDependencyInjection()
        {
            ExecuteBenchmark(MicrosoftDependencyInjectionContainer);
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

        #endregion

    }
}
