using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard
{
    [BenchmarkCategory("Standard")]
    public class SingletonBenchmark : BaseBenchmark
    {
        public static string Description =>
            @"Resolves a Singleton services from each container";

        [GlobalSetup]
        public void Setup()
        {
            var definitions = Definitions().ToArray();

            SetupContainerForTest(CreateAutofacContainer(), definitions);
            SetupContainerForTest(CreateGraceContainer(), definitions);
            SetupContainerForTest(CreateDryIocContainer(), definitions);
            SetupContainerForTest(CreateLightInjectContainer(), definitions);
            SetupContainerForTest(CreateMicrosoftDependencyInjectionContainer(), definitions);
            SetupContainerForTest(CreateNInjectContainer(), definitions);
            SetupContainerForTest(CreateSimpleInjectorContainer(), definitions);
            SetupContainerForTest(CreateStructureMapContainer(), definitions);
        }

        public static IEnumerable<RegistrationDefinition> Definitions()
        {
            yield return new RegistrationDefinition { ExportType = typeof(ISingletonService1), ActivationType = typeof(SingletonService1), RegistrationLifestyle = RegistrationLifestyle.Singleton };
            yield return new RegistrationDefinition { ExportType = typeof(ISingletonService2), ActivationType = typeof(SingletonService2), RegistrationLifestyle = RegistrationLifestyle.Singleton };
            yield return new RegistrationDefinition { ExportType = typeof(ISingletonService3), ActivationType = typeof(SingletonService3), RegistrationLifestyle = RegistrationLifestyle.Singleton };
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
            scope.Resolve(typeof(ISingletonService1));
            scope.Resolve(typeof(ISingletonService2));
            scope.Resolve(typeof(ISingletonService3));
        }
        #endregion
    }
}
