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
	public class OneArgFactoryBenchmark : StandardBenchmark
	{
	    public static string Description =>
	        "This benchmark registers a small object graph and a one argument function to create part of the object graph";

	    protected override IEnumerable<RegistrationDefinition> Definitions => SmallObjectServices.Definitions();

        /// <summary>
        /// Registers definitions and dummy classes for scope
        /// </summary>
        /// <param name="container"></param>
        /// <param name="definitions"></param>
        /// <param name="resolveStatements"></param>
        protected override void SetupContainerForTest(IContainer container, IEnumerable<RegistrationDefinition> definitions, params Action<IResolveScope>[] resolveStatements)
	    {
			container.RegisterFactory<ISmallObjectService, IOneArgeFactoryService>(service => new OneArgeFactoryService(service), RegistrationMode.Single, RegistrationLifestyle.Transient);

			base.SetupContainerForTest(container, definitions, resolveStatements);
		}
        
	    protected override void ExecuteBenchmark(IResolveScope scope)
	    {
	        scope.Resolve(typeof(IOneArgeFactoryService));
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
		[BenchmarkCategory(nameof(StructureMap))]
		public void StructureMap()
		{
			ExecuteBenchmark(StructureMapContainer);
		}
        
	}
}
