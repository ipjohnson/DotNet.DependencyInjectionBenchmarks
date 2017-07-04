using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Lifestyles
{
    [BenchmarkCategory("Lifestyles")]
    public class SingletonPerScopeBenchmark : BaseBenchmark
    {
        [GlobalSetup]
        public void Setup()
        {
            var definitions = SmallObjectBenchmark.Definitions(RegistrationLifestyle.SingletonPerScope).ToArray();

            var warmups = new Action<IResolveScope>[]
            {
                scope =>
                {
                    using (var childScope = scope.CreateScope())
                    {
                        childScope.Resolve(typeof(ISmallObjectGraphService1));
                    }
                },
                scope =>
                {
                    using (var childScope = scope.CreateScope())
                    {
                        childScope.Resolve(typeof(ISmallObjectGraphService2));
                    }
                },
                scope =>
                {
                    using (var childScope = scope.CreateScope())
                    {
                        childScope.Resolve(typeof(ISmallObjectGraphService3));
                    }
                }
            };

            SetupContainerForTest(CreateAutofacContainer(), definitions, warmups);
            SetupContainerForTest(CreateDryIocContainer(), definitions, warmups);
            SetupContainerForTest(CreateGraceContainer(), definitions, warmups);
            SetupContainerForTest(CreateLightInjectContainer(), definitions, warmups);
            SetupContainerForTest(CreateStructureMapContainer(), definitions, warmups);
        }

        #region Benchmarks

        [Benchmark]
        [BenchmarkCategory("Autofac")]
        public void Autofac()
        {
            ExecuteBenchmark(AutofacContainer);
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
        [BenchmarkCategory("StructureMap")]
        public void StructureMap()
        {
            ExecuteBenchmark(StructureMapContainer);
        }

        private void ExecuteBenchmark(IResolveScope scope)
        {
            using (var childScope = scope.CreateScope())
            {
                childScope.Resolve(typeof(ISmallObjectGraphService1));
                childScope.Resolve(typeof(ISmallObjectGraphService2));
                childScope.Resolve(typeof(ISmallObjectGraphService3));
            }
        }

        #endregion
    }
}
