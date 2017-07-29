using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Func
{
    [BenchmarkCategory("Func")]
    public class ThreeArgFuncBenchmark : BaseBenchmark
    {
        public static string Description =>
            "This benchmark registers a small object then resolves a three argument function for each object";

        [GlobalSetup]
        public void Setup()
        {
            var definitions = Definitions().ToArray();

            var warmupStatements = new Action<IResolveScope>[]
            {
                scope => scope.Resolve<Func<int,string,ITransientService,IThreeArgService1>>()(5, "Hello", new TransientService()),
            };

            SetupContainerForTest(CreateDryIocContainer(), definitions, warmupStatements);
            SetupContainerForTest(CreateGraceContainer(), definitions, warmupStatements);
        }

        private IEnumerable<RegistrationDefinition> Definitions()
        {
            yield return new RegistrationDefinition
            {
                ExportType = typeof(IThreeArgService1),
                ActivationType = typeof(ThreeArgService1)
            };
        }

        #region Benchmark

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

        private void ExecuteBenchmark(IResolveScope scope)
        {
            scope.Resolve<Func<int, string, ITransientService, IThreeArgService1>>()(5, "Hello",
                new TransientService());
        }

        #endregion
    }
}
