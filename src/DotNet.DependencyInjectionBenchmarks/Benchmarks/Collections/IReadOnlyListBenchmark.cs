﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Collections
{
    [BenchmarkCategory("Collections")]
    public class IReadOnlyListBenchmark : BaseBenchmark
    {

        [GlobalSetup]
        public void Setup()
        {
            var definitions = IEnumerableBenchmark.Definitions().ToArray();

            var warmup = new Action<IResolveScope>[]
            {
                scope => scope.Resolve<IReadOnlyList<IEnumerableService>>()
            };

            SetupScopeForTest(CreateAutofacScope(), definitions, warmup);
            SetupScopeForTest(CreateDryIocScope(), definitions, warmup);
            SetupScopeForTest(CreateGraceScope(), definitions, warmup);
            SetupScopeForTest(CreateLightInjectScope(), definitions, warmup);
        }

        #region Benchmarks

        [Benchmark]
        [BenchmarkCategory("Autofac")]
        public void Autofac()
        {
            ExecuteBenchmark(AutofacScope);
        }

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

        [Benchmark]
        [BenchmarkCategory("LightInject")]
        public void LightInject()
        {
            ExecuteBenchmark(LightInjectScope);
        }

        private void ExecuteBenchmark(IResolveScope scope)
        {
            if (scope.Resolve<IReadOnlyList<IEnumerableService>>().Count() != 5)
            {
                throw new Exception("Count does not equal 5");
            }
        }

        #endregion

    }
}
