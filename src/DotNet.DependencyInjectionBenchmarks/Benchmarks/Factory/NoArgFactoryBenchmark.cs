using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Factory
{
    [BenchmarkCategory("Factory")]
    public class NoArgFactoryBenchmark : BaseBenchmark
    {
        [GlobalSetup]
        public void Setup()
        {
            var definitions = Definitions().ToArray();

            SetupContainer(CreateAutofacScope(), definitions);
            SetupContainer(CreateDryIocScope(), definitions);
            SetupContainer(CreateGraceScope(), definitions);
            SetupContainer(CreateLightInjectScope(), definitions);
        }

        private void SetupContainer(IContainerScope container, RegistrationDefinition[] definitions)
        {
            container.RegisterFactory<ITransientService1>(() => new TransientService1(), RegistrationMode.Single, RegistrationLifestyle.Transient);

            SetupScopeForTest(container,definitions,
                scope => scope.Resolve<ISmallObjectGraphService1>(),
                scope => scope.Resolve<ISmallObjectGraphService2>(),
                scope => scope.Resolve<ISmallObjectGraphService3>()
            );
        }

        private IEnumerable<RegistrationDefinition> Definitions()
        {
            yield return new RegistrationDefinition { ExportType = typeof(ISmallObjectGraphService1), ActivationType = typeof(SmallObjectGraphService1)};
            yield return new RegistrationDefinition { ExportType = typeof(ISmallObjectGraphService2), ActivationType = typeof(SmallObjectGraphService3) };
            yield return new RegistrationDefinition { ExportType = typeof(ISmallObjectGraphService3), ActivationType = typeof(SmallObjectGraphService3) };

            foreach (var definition in SingletonBenchmark.Definitions())
            {
                yield return definition;
            }
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
            scope.Resolve<ISmallObjectGraphService1>();
            scope.Resolve<ISmallObjectGraphService2>();
            scope.Resolve<ISmallObjectGraphService3>();
        }

        #endregion
    }
}
