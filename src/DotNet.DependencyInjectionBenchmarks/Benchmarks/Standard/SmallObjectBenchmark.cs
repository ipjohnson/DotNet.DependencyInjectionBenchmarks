using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard
{
	[BenchmarkCategory("Standard")]
	public class SmallObjectBenchmark : StandardBenchmark
	{
		public static string Description =>
			@"Resolves a small object graph containing two transients and a Singleton from each container";

        protected override IEnumerable<RegistrationDefinition> Definitions => SmallObjectServices.Definitions();

        protected override void ExecuteBenchmark(IResolveScope scope)
	    {
	        scope.Resolve(typeof(ISmallObjectService));
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
		[BenchmarkCategory(nameof(NInject))]
		public void NInject()
		{
			ExecuteBenchmark(NInjectContainer);
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
