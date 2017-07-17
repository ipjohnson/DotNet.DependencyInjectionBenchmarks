using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Func
{
    [BenchmarkCategory("Func")]
    public class OneArgFuncBenchmark : BaseBenchmark
    {
        public static string Description =>
            "This benchmark registers a small object then resolves a one argument function for each object";

        [GlobalSetup]
        public void Setup()
        {
            var definitions = Definitions().ToArray();

            var warmups = new Action<IResolveScope>[]
            {
                r =>  r.Resolve<Func<ITransientService,ISmallObjectService>>()(new TransientService())
            };
            
            SetupContainerForTest(CreateDryIocContainer(), definitions, warmups);
            SetupContainerForTest(CreateGraceContainer(), definitions, warmups);
        }

        private IEnumerable<RegistrationDefinition> Definitions()
        {
            yield return new RegistrationDefinition { ExportType = typeof(ISmallObjectService), ActivationType = typeof(SmallObjectService) };
            
            foreach (var definition in SingletonBenchmark.Definitions())
            {
                yield return definition;
            }
        }

        #region Benchmark
        
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

        private void ExecuteBenchmark(IResolveScope scope)
        {
            scope.Resolve<Func<ITransientService, ISmallObjectService>>()(new TransientService());
        }

        #endregion
    }
}
