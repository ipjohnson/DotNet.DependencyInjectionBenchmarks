using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Collections
{
    [BenchmarkCategory("Collections")]
    public class IEnumerableBenchmark : BaseBenchmark
    {
        [GlobalSetup]
        public void Setup()
        {
            var definitions = Definitions().ToArray();

            var warmup = new Action<IResolveScope>[]
            {
                scope => scope.Resolve<IEnumerable<IEnumerableService>>()
            };

            SetupScopeForTest(CreateAutofacScope(), definitions,warmup );
            SetupScopeForTest(CreateDryIocScope(), definitions, warmup);
            SetupScopeForTest(CreateGraceScope(), definitions, warmup);
            SetupScopeForTest(CreateLightInjectScope(), definitions, warmup);
        }

        public static IEnumerable<RegistrationDefinition> Definitions()
        {
            yield return new RegistrationDefinition { ExportType = typeof(IEnumerableService), ActivationType = typeof(EnumerableService1), RegistrationMode = RegistrationMode.Multiple };
            yield return new RegistrationDefinition { ExportType = typeof(IEnumerableService), ActivationType = typeof(EnumerableService2), RegistrationMode = RegistrationMode.Multiple };
            yield return new RegistrationDefinition { ExportType = typeof(IEnumerableService), ActivationType = typeof(EnumerableService3), RegistrationMode = RegistrationMode.Multiple };
            yield return new RegistrationDefinition { ExportType = typeof(IEnumerableService), ActivationType = typeof(EnumerableService4), RegistrationMode = RegistrationMode.Multiple };
            yield return new RegistrationDefinition { ExportType = typeof(IEnumerableService), ActivationType = typeof(EnumerableService5), RegistrationMode = RegistrationMode.Multiple };
            yield return new RegistrationDefinition { ExportType = typeof(ISingletonService1), ActivationType = typeof(SingletonService1) };
            yield return new RegistrationDefinition { ExportType = typeof(ITransientService1), ActivationType = typeof(TransientService1) };
        }

        #region Benchmarks

        [Benchmark]
        public void Autofac()
        {
            ExecuteBenchmark(AutofacScope);
        }

        [Benchmark]
        public void DryIoc()
        {
            ExecuteBenchmark(DryIocScope);
        }

        [Benchmark]
        public void Grace()
        {
            ExecuteBenchmark(GraceScope);
        }

        [Benchmark]
        public void LightInject()
        {
            ExecuteBenchmark(LightInjectScope);
        }

        private void ExecuteBenchmark(IResolveScope scope)
        {
            if (scope.Resolve<IEnumerable<IEnumerableService>>().Count() != 5)
            {
                throw new Exception("Count does not equal 5");
            }
        }

        #endregion
    }
}
