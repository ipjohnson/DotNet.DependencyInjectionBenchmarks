using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Func
{
    [BenchmarkCategory("Func")]
    public class OneArgFuncBenchmark : BaseBenchmark
    {
        [GlobalSetup]
        public void Setup()
        {
            var definitions = Definitions().ToArray();

            var warmups = new Action<IResolveScope>[]
            {
                r =>  r.Resolve<Func<ITransientService1,ISmallObjectGraphService1>>()(new TransientService1()),
                r =>  r.Resolve<Func<ITransientService2,ISmallObjectGraphService2>>()(new TransientService2()),
                r =>  r.Resolve<Func<ITransientService3,ISmallObjectGraphService3>>()(new TransientService3())
            };
            
            SetupScopeForTest(CreateDryIocScope(), definitions, warmups);
            SetupScopeForTest(CreateGraceScope(), definitions, warmups);
        }

        private IEnumerable<RegistrationDefinition> Definitions()
        {
            yield return new RegistrationDefinition { ExportType = typeof(ISmallObjectGraphService1), ActivationType = typeof(SmallObjectGraphService1) };
            yield return new RegistrationDefinition { ExportType = typeof(ISmallObjectGraphService2), ActivationType = typeof(SmallObjectGraphService2) };
            yield return new RegistrationDefinition { ExportType = typeof(ISmallObjectGraphService3), ActivationType = typeof(SmallObjectGraphService3) };

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
            ExecuteBenchmark(DryIocScope);
        }

        [Benchmark]
        [BenchmarkCategory("Grace")]
        public void Grace()
        {
            ExecuteBenchmark(GraceScope);
        }

        private void ExecuteBenchmark(IResolveScope scope)
        {
            scope.Resolve<Func<ITransientService1, ISmallObjectGraphService1>>()(new TransientService1());
            scope.Resolve<Func<ITransientService2, ISmallObjectGraphService2>>()(new TransientService2());
            scope.Resolve<Func<ITransientService3, ISmallObjectGraphService3>>()(new TransientService3());
        }

        #endregion
    }
}
