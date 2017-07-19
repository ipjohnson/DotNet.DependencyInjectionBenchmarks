using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard
{
    [BenchmarkCategory("Standard")]
    public class LargeObjectBenchmark : BaseBenchmark
    {
        public static string Description =>
            @"Resolves a Large object graph containing 7 transients and 3 Singletons from each container";

        [GlobalSetup]
        public void Setup()
        {
            var definitions = Definitions().ToArray();

            SetupContainerForTest(CreateAutofacContainer(), definitions);
            SetupContainerForTest(CreateCastleWindsorContainer(), definitions);
            SetupContainerForTest(CreateDryIocContainer(), definitions);
            SetupContainerForTest(CreateGraceContainer(), definitions);
            SetupContainerForTest(CreateLightInjectContainer(), definitions);
            SetupContainerForTest(CreateMicrosoftDependencyInjectionContainer(), definitions);
            SetupContainerForTest(CreateNInjectContainer(), definitions);
            SetupContainerForTest(CreateSimpleInjectorContainer(), definitions);
            SetupContainerForTest(CreateStructureMapContainer(), definitions);
        }

        public static IEnumerable<RegistrationDefinition> Definitions()
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

        #region Benchmarks

        [Benchmark]
        [BenchmarkCategory("Autofac")]
        public void Autofac()
        {
            ExecuteBenchmark(AutofacContainer);
        }

        [Benchmark]
        [BenchmarkCategory("CastleWindsor")]
        public void CastleWindsor()
        {
            ExecuteBenchmark(CastleWindsorContainer);
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
        [BenchmarkCategory("MicrosoftDependencyInjection")]
        public void MicrosoftDependencyInjection()
        {
            ExecuteBenchmark(MicrosoftDependencyInjectionContainer);
        }

        [Benchmark]
        [BenchmarkCategory("NInject")]
        public void NInject()
        {
            ExecuteBenchmark(NInjectContainer);
        }

        [Benchmark]
        [BenchmarkCategory("SimpleInjector")]
        public void SimpleInjector()
        {
            ExecuteBenchmark(SimpleInjectorContainer);
        }

        [Benchmark]
        [BenchmarkCategory("StructureMap")]
        public void StructureMap()
        {
            ExecuteBenchmark(StructureMapContainer);
        }

        private void ExecuteBenchmark(IResolveScope scope)
        {
            scope.Resolve(typeof(ILargeComplexService));
        }

        #endregion

    }
}
