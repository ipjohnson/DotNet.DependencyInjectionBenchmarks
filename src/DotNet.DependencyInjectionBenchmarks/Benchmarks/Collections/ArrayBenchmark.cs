using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Collections
{
    [BenchmarkCategory("Collections")]
    public class ArrayBenchmark : BaseBenchmark
    {
        [GlobalSetup]
        public void Setup()
        {
            var definitions =IEnumerableBenchmark.Definitions().ToArray();

            var warmup = new Action<IResolveScope>[]
            {
                scope => scope.Resolve<IEnumerableService[]>()
            };

            SetupScopeForTest(CreateAutofacScope(), definitions, warmup);;
            SetupScopeForTest(CreateGraceScope(), definitions, warmup);
            SetupScopeForTest(CreateLightInjectScope(), definitions, warmup);
        }
        
        #region Benchmarks

        [Benchmark]
        public void Autofac()
        {
            ExecuteBenchmark(AutofacScope);
        }
        
        [Benchmark]
        public void Grace()
        {
            ExecuteBenchmark(GraceScope);
        }

        [Benchmark]
        public void LightInject()
        {
            ExecuteBenchmark(LightInjectScope);
        }

        private void ExecuteBenchmark(IResolveScope scope)
        {
            if (scope.Resolve<IEnumerableService[]>().Count() != 5)
            {
                throw new Exception("Count does not equal 5");
            }
        }

        #endregion
    }
}
