using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Lifestyles
{
	[BenchmarkCategory("Lifestyles")]
	public class SingletonPerObjectGraphBenchmark : BaseBenchmark
	{
		public static string Description =>
			@"This benchmark registers a small object as Singleton Per Object Graph then resolves it as part of a slightly larger object graph";

		[GlobalSetup]
		public void Setup()
		{
			var definitions = SmallObjectBenchmark.Definitions(RegistrationLifestyle.SingletonPerObjectGraph, typeof(IImportMultipleSmallObject)).ToList();

		    definitions.Add(new RegistrationDefinition { ExportType = typeof(IImportMultipleSmallObject), ActivationType = typeof(ImportMultipleSmallObject) });

            var warmups = new Action<IResolveScope>[]
			{
			    scope =>
			    {
			        var instance = scope.Resolve<IImportMultipleSmallObject>();
                    
			        if (!ReferenceEquals(instance.SmallObject1, instance.SmallObject2))
			        {
			            throw new Exception("Not the same instance");
			        }
                }
			};

			SetupContainerForTest(CreateCastleWindsorContainer(), definitions, warmups);
			SetupContainerForTest(CreateGraceContainer(), definitions, warmups);
			SetupContainerForTest(CreateStructureMapContainer(), definitions, warmups);
		}

		#region Benchmarks

		[Benchmark]
		[BenchmarkCategory(nameof(CastleWindsor))]
		public void CastleWindsor()
		{
			ExecuteBenchmark(CastleWindsorContainer);
		}
		
		[Benchmark]
		[BenchmarkCategory(nameof(Grace))]
		public void Grace()
		{
			ExecuteBenchmark(GraceContainer);
		}
		
		[Benchmark]
		[BenchmarkCategory(nameof(StructureMap))]
		public void StructureMap()
		{
			ExecuteBenchmark(StructureMapContainer);
		}

		private void ExecuteBenchmark(IResolveScope scope)
		{
            scope.Resolve(typeof(IImportMultipleSmallObject));
        }

		#endregion

	}
}
