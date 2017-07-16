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
	public class OneArgFactoryBenchmark : BaseBenchmark
	{
		[GlobalSetup]
		public void Setup()
		{
			var definitions = SmallObjectBenchmark.Definitions().ToArray();

			SetupContainer(CreateAutofacContainer(), definitions);
			SetupContainer(CreateCastleWindsorContainer(), definitions);
			SetupContainer(CreateDryIocContainer(), definitions);
			SetupContainer(CreateGraceContainer(), definitions);
			SetupContainer(CreateLightInjectContainer(), definitions);
			SetupContainer(CreateMicrosoftDependencyInjectionContainer(), definitions);
			//SetupContainer(CreateSimpleInjectorContainer(), definitions);
			SetupContainer(CreateStructureMapContainer(), definitions);
		}

		private void SetupContainer(IContainer container, RegistrationDefinition[] definitions)
		{
			container.RegisterFactory<ISmallObjectService, IOneArgeFactoryService>(service => new OneArgeFactoryService(service), RegistrationMode.Single, RegistrationLifestyle.Transient);

			SetupContainerForTest(container, definitions, scope => scope.Resolve(typeof(IOneArgeFactoryService)));
		}


		#region Benchmarks

		[Benchmark]
		[BenchmarkCategory("Autofac")]
		public void Autofac()
		{
			ExecuteBenchmark(AutofacContainer);
		}

		[Benchmark]
		[BenchmarkCategory("CastleWindsor")]
		public void CastleWindsor()
		{
			ExecuteBenchmark(CastleWindsorContainer);
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
		[BenchmarkCategory("MicrosoftDependencyInjection")]
		public void MicrosoftDependencyInjection()
		{
			ExecuteBenchmark(MicrosoftDependencyInjectionContainer);
		}

		[Benchmark]
		[BenchmarkCategory("StructureMap")]
		public void StructureMap()
		{
			ExecuteBenchmark(StructureMapContainer);
		}

		private void ExecuteBenchmark(IResolveScope scope)
		{
			scope.Resolve(typeof(IOneArgeFactoryService));
		}

		#endregion
	}
}
