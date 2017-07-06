using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Parameters;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Func
{
    [BenchmarkCategory("Func")]
    public class ThreeArgFuncBenchmark : BaseBenchmark
    {
        public static string Description =>
            "This benchmark registers 3 small objects then resolves a three argument function for each object";

        [GlobalSetup]
        public void Setup()
        {
            var definitions = Definitions().ToArray();

            var warmupStatements = new Action<IResolveScope>[]
            {
                scope => scope.Resolve<Func<int,string,ITransientService1,IThreeArgService1>>()(5, "Hello", new TransientService1()),
                scope => scope.Resolve<Func<int,string,ITransientService2,IThreeArgService2>>()(10, "Good", new TransientService2()),
                scope => scope.Resolve<Func<int,string,ITransientService3,IThreeArgService3>>()(15, "Bye", new TransientService3())
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
            
            yield return new RegistrationDefinition
            {
                ExportType = typeof(IThreeArgService2),
                ActivationType = typeof(ThreeArgService2)
            };
            
            yield return new RegistrationDefinition
            {
                ExportType = typeof(IThreeArgService3),
                ActivationType = typeof(ThreeArgService3)
            };
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
            scope.Resolve<Func<int, string, ITransientService1, IThreeArgService1>>()(5, "Hello",
                new TransientService1());
            scope.Resolve<Func<int, string, ITransientService2, IThreeArgService2>>()(10, "Good",
                new TransientService2());
            scope.Resolve<Func<int, string, ITransientService3, IThreeArgService3>>()(15, "Bye",
                new TransientService3());
        }

        #endregion
    }
}
