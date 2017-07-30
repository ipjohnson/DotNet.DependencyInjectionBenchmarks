using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Factory
{
    [BenchmarkCategory("Factory")]
    public class NoArgFactoryBenchmark : StandardBenchmark
    {
        public static string Description =>
            "This benchmark registers a small object using a factory to provide one piece of the object graph.";
        
        protected override void SetupContainerForTest(IContainer container, IEnumerable<RegistrationDefinition> definitions, params Action<IResolveScope>[] resolveStatements)
        {
            container.RegisterFactory<ITransientService>(() => new TransientService(), RegistrationMode.Single, RegistrationLifestyle.Transient);

            base.SetupContainerForTest(container, definitions, resolveStatements);
        }

        protected override IEnumerable<RegistrationDefinition> Definitions
        {
            get
            {
                yield return new RegistrationDefinition { ExportType = typeof(ISmallObjectService), ActivationType = typeof(SmallObjectService) };

                yield return new RegistrationDefinition { ExportType = typeof(ISingletonService), ActivationType = typeof(SingletonService), RegistrationLifestyle = RegistrationLifestyle.Singleton };
            }
        }

        protected override void ExecuteBenchmark(IResolveScope scope)
        {
            scope.Resolve<ISmallObjectService>();
        }
        
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
    }
}
