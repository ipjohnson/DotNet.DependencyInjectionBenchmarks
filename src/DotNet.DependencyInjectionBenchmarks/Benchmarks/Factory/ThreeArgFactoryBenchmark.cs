using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Factory
{
    [BenchmarkCategory("Factory")]
    public class ThreeArgFactoryBenchmark : BaseBenchmark
	{
		[GlobalSetup]
		public void Setup()
		{
			SetupContainer(CreateAutofacContainer());
			SetupContainer(CreateCastleWindsorContainer());
			SetupContainer(CreateDryIocContainer());
			SetupContainer(CreateGraceContainer());
			//SetupContainer(CreateLightInjectContainer());
			SetupContainer(CreateMicrosoftDependencyInjectionContainer());
			SetupContainer(CreateStructureMapContainer());
		}

		private void SetupContainer(IContainer container)
		{
			container.RegisterFactory<IThreeArgTransient1, IThreeArgTransient2, IThreeArgTransient3, IThreeArgRefService>((transient1, transient2, transient3) => new ThreeArgRefService(transient1, transient2, transient3), RegistrationMode.Single, RegistrationLifestyle.Transient);

			SetupContainerForTest(container,
				new[]
				{
					new RegistrationDefinition { ExportType = typeof(IThreeArgTransient1), ActivationType = typeof(ThreeArgTransient1) },
					new RegistrationDefinition { ExportType = typeof(IThreeArgTransient2), ActivationType = typeof(ThreeArgTransient2) },
					new RegistrationDefinition { ExportType = typeof(IThreeArgTransient3), ActivationType = typeof(ThreeArgTransient3) }
				}, scope => scope.Resolve(typeof(IThreeArgRefService)));
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
			scope.Resolve(typeof(IThreeArgRefService));
		}

		#endregion
	}
}
