using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Lifestyles
{
    [BenchmarkCategory("Lifestyles")]
    public class SingletonPerScopeBenchmark : StandardBenchmark
    {
        public static string Description =>
            @"This benchmark registers a small object as Singleton Per Scope then creates a scope and resolves the small object.";
        
        protected override IEnumerable<RegistrationDefinition> Definitions
        {
            get
            {
                foreach (var definition in SmallObjectServices.Definitions(RegistrationLifestyle.SingletonPerScope))
                {
                    yield return definition;
                }

                yield return new RegistrationDefinition
                {
                    ExportType = typeof(IImportMultipleSmallObject),
                    ActivationType = typeof(ImportMultipleSmallObject)
                };
            }
        }

        protected override void Warmup(IResolveScope scope)
        {
            using (var childScope = scope.CreateScope())
            {
                var instance = childScope.Resolve<IImportMultipleSmallObject>();

                if (!ReferenceEquals(instance.SmallObject1, instance.SmallObject2))
                {
                    throw new Exception("Not the same instance");
                }

                var instance2 = childScope.Resolve<IImportMultipleSmallObject>();

                if (ReferenceEquals(instance, instance2))
                {
                    throw new Exception("Same instance");
                }

                if (!ReferenceEquals(instance.SmallObject1, instance2.SmallObject1))
                {
                    throw new Exception("Small object1 not same instance");
                }

                if (!ReferenceEquals(instance.SmallObject2, instance2.SmallObject2))
                {
                    throw new Exception("Small object2 not same instance");
                }
            }
        }

        protected override void ExecuteBenchmark(IResolveScope scope)
        {
            using (var childScope = scope.CreateScope())
            {
                childScope.Resolve(typeof(IImportMultipleSmallObject));
            }
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
