using System.Linq;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Scoped
{
    [BenchmarkCategory("Scoped")]
    public class CreateScopeAndResolveBenchmark : BaseBenchmark
    {
        public static string Description =>
            "This benchmark creates a lifetime scope and then resolves a small object from the child scope.";

        [GlobalSetup]
        public void Setup()
        {
            var definitions = SmallObjectBenchmark.Definitions().ToList();
        
            SetupContainerForTest(CreateAutofacContainer(), definitions);
            SetupContainerForTest(CreateCastleWindsorContainer(), definitions);
            SetupContainerForTest(CreateDryIocContainer(), definitions);
            SetupContainerForTest(CreateGraceContainer(), definitions);
            SetupContainerForTest(CreateLightInjectContainer(), definitions);
            SetupContainerForTest(CreateMicrosoftDependencyInjectionContainer(), definitions);
            SetupContainerForTest(CreateStructureMapContainer(), definitions);
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
            using (var childScope = scope.CreateScope())
            {
                childScope.Resolve(typeof(ISmallObjectService));
            }
        }

        #endregion
    }
}
