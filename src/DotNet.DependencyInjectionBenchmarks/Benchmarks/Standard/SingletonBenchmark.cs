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
            @"Resolves 3 Singleton services from each container";

        [GlobalSetup]
        public void Setup()
        {
            var definitions = Definitions().ToArray();

            SetupScopeForTest(CreateAutofacScope(), definitions);
            SetupScopeForTest(CreateGraceScope(), definitions);
            SetupScopeForTest(CreateDryIocScope(), definitions);
            SetupScopeForTest(CreateLightInjectScope(), definitions);
            SetupScopeForTest(CreateSimpleInjectorContainerScope(), definitions);
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
            scope.Resolve(typeof(ISingletonService1));
            scope.Resolve(typeof(ISingletonService2));
            scope.Resolve(typeof(ISingletonService3));
        }
        #endregion
    }
}
