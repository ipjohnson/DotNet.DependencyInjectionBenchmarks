using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Collections
{
    [BenchmarkCategory("Collections")]
    public class IReadOnlyListBenchmark : BaseBenchmark
    {
        #region Benchmark Definition

        public static string Description =>
            "This benchmark registers 5 small objects then resolves them as an IReadOnlyList(T).";

        private void ExecuteBenchmark(IResolveScope scope)
        {
            if (scope.Resolve<IReadOnlyList<IEnumerableService>>().Count() != 5)
            {
                throw new Exception("Count does not equal 5");
            }
        }

        #endregion

        #region Autofac

        [GlobalSetup(Target = nameof(Autofac))]
        public void AutofacSetup()
        {
            SetupContainerForTest(CreateAutofacContainer(), IEnumerableBenchmark.Definitions(), ExecuteBenchmark);
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
            SetupContainerForTest(CreateDryIocContainer(), IEnumerableBenchmark.Definitions(), ExecuteBenchmark);
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
            SetupContainerForTest(CreateGraceContainer(), IEnumerableBenchmark.Definitions(), ExecuteBenchmark);
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
            SetupContainerForTest(CreateLightInjectContainer(), IEnumerableBenchmark.Definitions(), ExecuteBenchmark);
        }

        [Benchmark]
        [BenchmarkCategory(nameof(LightInject))]
        public void LightInject()
        {
            ExecuteBenchmark(LightInjectContainer);
        }

        #endregion

        #region SimpleInjector

        [GlobalSetup(Target = nameof(SimpleInjector))]
        public void SimpleInjectorSetup()
        {
            SetupContainerForTest(CreateSimpleInjectorContainer(), IEnumerableBenchmark.Definitions(), ExecuteBenchmark);
        }

        [Benchmark]
        [BenchmarkCategory(nameof(SimpleInjector))]
        public void SimpleInjector()
        {
            ExecuteBenchmark(SimpleInjectorContainer);
        }
        #endregion
    }
}
