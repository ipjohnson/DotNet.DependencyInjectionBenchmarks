using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard
{
	[BenchmarkCategory("Standard")]
	public class SmallObjectBenchmark : BaseBenchmark
	{
		public static string Description =>
			@"Resolves a small object graph from each container";

		#region setup
		[GlobalSetup]
		public void Setup()
		{
			var definitions = Definitions().ToArray();

			SetupContainerForTest(CreateAutofacContainer(), definitions);
			SetupContainerForTest(CreateCastleWindsorContainer(), definitions);
			SetupContainerForTest(CreateDryIocContainer(), definitions);
			SetupContainerForTest(CreateGraceContainer(), definitions);
			SetupContainerForTest(CreateLightInjectContainer(), definitions);
			SetupContainerForTest(CreateMicrosoftDependencyInjectionContainer(), definitions);
			SetupContainerForTest(CreateNInjectContainer(), definitions);
			SetupContainerForTest(CreateSimpleInjectorContainer(), definitions);
			SetupContainerForTest(CreateStructureMapContainer(), definitions);
		}
		#endregion

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
		[BenchmarkCategory("NInject")]
		public void NInject()
		{
			ExecuteBenchmark(NInjectContainer);
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
			scope.Resolve(typeof(ISmallObjectService));
		}

		#endregion

		#region Test definition

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="lifestyle">default lifestyle for the Small object graph</param>
	    /// <param name="lifestyleInfo"></param>
	    /// <returns></returns>
	    public static IEnumerable<RegistrationDefinition> Definitions(RegistrationLifestyle lifestyle = RegistrationLifestyle.Transient, object lifestyleInfo = null)
		{
			var singletons = SingletonBenchmark.Definitions().ToArray();

			yield return singletons[0];
			yield return new RegistrationDefinition { ExportType = typeof(ISmallObjectService), ActivationType = typeof(SmallObjectService), RegistrationLifestyle = lifestyle, LifestyleInformation = lifestyleInfo };
			yield return new RegistrationDefinition { ExportType = typeof(ITransientService), ActivationType = typeof(TransientService) };
		}

		#endregion
	}
}
