using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Factory
{
    [BenchmarkCategory("Factory")]
    public class ThreeArgFactoryBenchmark : StandardBenchmark
    {
        public static string Description =>
            "This benchmark resolves a small object graph using a factory that takes 3 arguments.";

        /// <summary>
        /// Registers definitions and dummy classes for scope
        /// </summary>
        /// <param name="container"></param>
        /// <param name="definitions"></param>
        /// <param name="resolveStatements"></param>
        protected override void SetupContainerForTest(IContainer container, IEnumerable<RegistrationDefinition> definitions, params Action<IResolveScope>[] resolveStatements)
        {
            container.RegisterFactory<IThreeArgTransient1, IThreeArgTransient2, IThreeArgTransient3, IThreeArgRefService>((transient1, transient2, transient3) => new ThreeArgRefService(transient1, transient2, transient3), RegistrationMode.Single, RegistrationLifestyle.Transient);

            base.SetupContainerForTest(container, definitions, scope => scope.Resolve(typeof(IThreeArgRefService)));
        }
        
        protected override IEnumerable<RegistrationDefinition> Definitions =>
            new[]
            {
                new RegistrationDefinition { ExportType = typeof(IThreeArgTransient1), ActivationType = typeof(ThreeArgTransient1) },
                new RegistrationDefinition { ExportType = typeof(IThreeArgTransient2), ActivationType = typeof(ThreeArgTransient2) },
                new RegistrationDefinition { ExportType = typeof(IThreeArgTransient3), ActivationType = typeof(ThreeArgTransient3) }
            };

        protected override void ExecuteBenchmark(IResolveScope scope)
        {
            scope.Resolve(typeof(IThreeArgRefService));
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
        [BenchmarkCategory(nameof(MicrosoftDependencyInjection))]
        public void MicrosoftDependencyInjection()
        {
            ExecuteBenchmark(MicrosoftDependencyInjectionContainer);
        }

        [Benchmark]
        [BenchmarkCategory(nameof(StructureMap))]
        public void StructureMap()
        {
            ExecuteBenchmark(StructureMapContainer);
        }
    }
}
