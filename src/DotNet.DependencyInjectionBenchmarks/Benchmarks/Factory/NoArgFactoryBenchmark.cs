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
        public static string Description =>
            "This benchmark registers 3 small objects using a factory to provide on piece of the object graph.";
        
        [GlobalSetup]
        public void Setup()
        {
            var definitions = Definitions().ToArray();

            SetupContainer(CreateAutofacContainer(), definitions);
            SetupContainer(CreateDryIocContainer(), definitions);
            SetupContainer(CreateGraceContainer(), definitions);
            SetupContainer(CreateLightInjectContainer(), definitions);
            SetupContainer(CreateSimpleInjectorContainer(), definitions);
            SetupContainer(CreateStructureMapContainer(), definitions);
        }

        private void SetupContainer(IContainer container, RegistrationDefinition[] definitions)
        {
            container.RegisterFactory<ITransientService1>(() => new TransientService1(), RegistrationMode.Single, RegistrationLifestyle.Transient);
            container.RegisterFactory<ITransientService2>(() => new TransientService2(), RegistrationMode.Single, RegistrationLifestyle.Transient);
            container.RegisterFactory<ITransientService3>(() => new TransientService3(), RegistrationMode.Single, RegistrationLifestyle.Transient);

            SetupContainerForTest(container,definitions,
                scope => scope.Resolve<ISmallObjectGraphService1>(),
                scope => scope.Resolve<ISmallObjectGraphService2>(),
                scope => scope.Resolve<ISmallObjectGraphService3>()
            );
        }

        private IEnumerable<RegistrationDefinition> Definitions()
        {
            yield return new RegistrationDefinition { ExportType = typeof(ISmallObjectGraphService1), ActivationType = typeof(SmallObjectGraphService1)};
            yield return new RegistrationDefinition { ExportType = typeof(ISmallObjectGraphService2), ActivationType = typeof(SmallObjectGraphService2) };
            yield return new RegistrationDefinition { ExportType = typeof(ISmallObjectGraphService3), ActivationType = typeof(SmallObjectGraphService3) };

            foreach (var definition in SingletonBenchmark.Definitions())
            {
                yield return definition;
            }
        }

        #region Benchmarks

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
            scope.Resolve<ISmallObjectGraphService1>();
            scope.Resolve<ISmallObjectGraphService2>();
            scope.Resolve<ISmallObjectGraphService3>();
        }

        #endregion
    }
}
