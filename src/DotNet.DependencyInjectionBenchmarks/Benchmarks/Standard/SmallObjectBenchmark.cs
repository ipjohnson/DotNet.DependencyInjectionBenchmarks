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
    public class SmallObjectBenchmark : BaseBenchmark
    {
        public static string Description =>
            @"Resolves 3 small object graphs from each container";

        #region setup
        [GlobalSetup]
        public void Setup()
        {
            var definitions = Definitions().ToArray();

            SetupScopeForTest(CreateAutofacScope(), definitions);
            SetupScopeForTest(CreateGraceScope(), definitions);
            SetupScopeForTest(CreateDryIocScope(), definitions);
            SetupScopeForTest(CreateLightInjectScope(), definitions);
            SetupScopeForTest(CreateSimpleInjectorContainerScope(), definitions);
            SetupScopeForTest(CreateStructureMapContainer(), definitions);
        }
        #endregion

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
        [BenchmarkCategory("SimpleInjector")]
        public void SimpleInjector()
        {
            ExecuteBenchmark(SimpleInjectorScope);
        }

        [Benchmark]
        [BenchmarkCategory("StructureMap")]
        public void StructureMap()
        {
            ExecuteBenchmark(StructureMapContainer);
        }

        private void ExecuteBenchmark(IResolveScope scope)
        {
            scope.Resolve(typeof(ISmallObjectGraphService1));
            scope.Resolve(typeof(ISmallObjectGraphService2));
            scope.Resolve(typeof(ISmallObjectGraphService3));
        }

        #endregion

        #region Test definition

        public static IEnumerable<RegistrationDefinition> Definitions()
        {
            yield return new RegistrationDefinition { ExportType = typeof(ISmallObjectGraphService1), ActivationType = typeof(SmallObjectGraphService1) };
            yield return new RegistrationDefinition { ExportType = typeof(ITransientService1), ActivationType = typeof(TransientService1) };

            yield return new RegistrationDefinition { ExportType = typeof(ISmallObjectGraphService2), ActivationType = typeof(SmallObjectGraphService2) };
            yield return new RegistrationDefinition { ExportType = typeof(ITransientService2), ActivationType = typeof(TransientService2) };

            yield return new RegistrationDefinition { ExportType = typeof(ISmallObjectGraphService3), ActivationType = typeof(SmallObjectGraphService3) };
            yield return new RegistrationDefinition { ExportType = typeof(ITransientService3), ActivationType = typeof(TransientService3) };

            foreach (var definition in SingletonBenchmark.Definitions())
            {
                yield return definition;
            }
        }

        #endregion
    }
}
