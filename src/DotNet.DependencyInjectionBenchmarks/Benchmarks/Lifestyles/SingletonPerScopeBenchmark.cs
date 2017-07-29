using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Lifestyles
{
    [BenchmarkCategory("Lifestyles")]
    public class SingletonPerScopeBenchmark : BaseBenchmark
    {
        public static string Description =>
            @"This benchmark registers a small object as Singleton Per Scope then creates a scope and resolves the small object.";

        [GlobalSetup]
        public void Setup()
        {
            var definitions = SmallObjectBenchmark.Definitions(RegistrationLifestyle.SingletonPerScope).ToList();

            definitions.Add(new RegistrationDefinition{ ExportType = typeof(IImportMultipleSmallObject), ActivationType = typeof(ImportMultipleSmallObject)});

            var warmups = new Action<IResolveScope>[]
            {
                scope =>
                {
                    using (var childScope = scope.CreateScope())
                    {
                        var instance = childScope.Resolve<IImportMultipleSmallObject>();

                        if (!ReferenceEquals(instance.SmallObject1, instance.SmallObject2))
                        {
                            throw new Exception("Not the same instance");
                        }
                    }
                }
            };

            SetupContainerForTest(CreateAutofacContainer(), definitions, warmups);
            SetupContainerForTest(CreateCastleWindsorContainer(), definitions, warmups);
            SetupContainerForTest(CreateDryIocContainer(), definitions, warmups);
            SetupContainerForTest(CreateGraceContainer(), definitions, warmups);
            SetupContainerForTest(CreateLightInjectContainer(), definitions, warmups);
            SetupContainerForTest(CreateMicrosoftDependencyInjectionContainer(), definitions, warmups);
            SetupContainerForTest(CreateStructureMapContainer(), definitions, warmups);
        }

        #region Benchmarks

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

        private void ExecuteBenchmark(IResolveScope scope)
        {
            using (var childScope = scope.CreateScope())
            {
                childScope.Resolve(typeof(IImportMultipleSmallObject));
            }
        }

        #endregion
    }
}
