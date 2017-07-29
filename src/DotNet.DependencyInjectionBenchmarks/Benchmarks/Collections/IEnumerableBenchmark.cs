using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Collections
{
    [BenchmarkCategory("Collections")]
    public class IEnumerableBenchmark : BaseBenchmark
    {
        #region Benchmark Definition

        public static string Description =>
            "This benchmark registers 5 small objects then resolves them as an IEnumerable(T).";

        public static IEnumerable<RegistrationDefinition> Definitions()
        {
            yield return new RegistrationDefinition { ExportType = typeof(IEnumerableService), ActivationType = typeof(EnumerableService1), RegistrationMode = RegistrationMode.Multiple };
            yield return new RegistrationDefinition { ExportType = typeof(IEnumerableService), ActivationType = typeof(EnumerableService2), RegistrationMode = RegistrationMode.Multiple };
            yield return new RegistrationDefinition { ExportType = typeof(IEnumerableService), ActivationType = typeof(EnumerableService3), RegistrationMode = RegistrationMode.Multiple };
            yield return new RegistrationDefinition { ExportType = typeof(IEnumerableService), ActivationType = typeof(EnumerableService4), RegistrationMode = RegistrationMode.Multiple };
            yield return new RegistrationDefinition { ExportType = typeof(IEnumerableService), ActivationType = typeof(EnumerableService5), RegistrationMode = RegistrationMode.Multiple };
            yield return new RegistrationDefinition { ExportType = typeof(ISingletonService), ActivationType = typeof(SingletonService) };
            yield return new RegistrationDefinition { ExportType = typeof(ITransientService), ActivationType = typeof(TransientService) };
        }

        private void ExecuteBenchmark(IResolveScope scope)
        {
            if (scope.Resolve<IEnumerable<IEnumerableService>>().Count() != 5)
            {
                throw new Exception("Count does not equal 5");
            }
        }
        #endregion

        #region Autofac

        [GlobalSetup(Target = nameof(Autofac))]
        public void AutofacSetup()
        {
            SetupContainerForTest(CreateAutofacContainer(), Definitions(), ExecuteBenchmark);
        }

        [Benchmark]
        [BenchmarkCategory(nameof(Autofac))]
        public void Autofac()
        {
            ExecuteBenchmark(AutofacContainer);
        }
        #endregion

        #region DryIoc

        [GlobalSetup(Target = nameof(DryIoc))]
        public void DryIocSetup()
        {
            SetupContainerForTest(CreateDryIocContainer(), Definitions(), ExecuteBenchmark);
        }

        [Benchmark]
        [BenchmarkCategory(nameof(DryIoc))]
        public void DryIoc()
        {
            ExecuteBenchmark(DryIocContainer);
        }
        #endregion

        #region Grace

        [GlobalSetup(Target = nameof(Grace))]
        public void GraceSetup()
        {
            SetupContainerForTest(CreateGraceContainer(), Definitions(), ExecuteBenchmark);
        }

        [Benchmark]
        [BenchmarkCategory(nameof(Grace))]
        public void Grace()
        {
            ExecuteBenchmark(GraceContainer);
        }
        #endregion

        #region LightInject

        [GlobalSetup(Target = nameof(LightInject))]
        public void LightInjectSetup()
        {
            SetupContainerForTest(CreateLightInjectContainer(), Definitions(), ExecuteBenchmark);
        }

        [Benchmark]
        [BenchmarkCategory(nameof(LightInject))]
        public void LightInject()
        {
            ExecuteBenchmark(LightInjectContainer);
        }
        #endregion

        #region MicrosoftDependencyInjection

        [GlobalSetup(Target = nameof(MicrosoftDependencyInjection))]
        public void MicrosoftDependencyInjectionSetup()
        {
            SetupContainerForTest(CreateMicrosoftDependencyInjectionContainer(), Definitions(), ExecuteBenchmark);
        }

        [Benchmark]
        [BenchmarkCategory(nameof(MicrosoftDependencyInjection))]
        public void MicrosoftDependencyInjection()
        {
            ExecuteBenchmark(MicrosoftDependencyInjectionContainer);
        }
        #endregion

        #region SimpleInjector

        [GlobalSetup(Target = nameof(SimpleInjector))]
        public void SimpleInjectorSetup()
        {
            SetupContainerForTest(CreateSimpleInjectorContainer(), Definitions(), ExecuteBenchmark);
        }

        [Benchmark]
        [BenchmarkCategory(nameof(SimpleInjector))]
        public void SimpleInjector()
        {
            ExecuteBenchmark(SimpleInjectorContainer);
        }
        #endregion

        #region StructureMap

        [GlobalSetup(Target = nameof(StructureMap))]
        public void StructureMapSetup()
        {
            SetupContainerForTest(CreateStructureMapContainer(), Definitions(), ExecuteBenchmark);
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
