﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Lazy
{
    [BenchmarkCategory("Lazy")]
    public class LazyBenchmark : BaseBenchmark
    {
        [GlobalSetup]
        public void Setup()
        {
            var definitions = SmallObjectBenchmark.Definitions().ToArray();

            SetupScopeForTest(CreateAutofacScope(),definitions);
            //SetupScopeForTest(CreateDryIocScope(), definitions);
            SetupScopeForTest(CreateGraceScope(),definitions);
            SetupScopeForTest(CreateLightInjectScope(), definitions);
            SetupScopeForTest(CreateSimpleInjectorContainerScope(), definitions);
        }

        [Benchmark]
        public void Autofac()
        {
            ExecuteBenchmark(AutofacScope);
        }

        //[Benchmark]
        //public void DryIoc()
        //{
        //    ExecuteBenchmark(DryIocScope);
        //}

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
        
        [Benchmark]
        public void SimpleInjector()
        {
            ExecuteBenchmark(LightInjectScope);
        }

        private void ExecuteBenchmark(IResolveScope scope)
        {
            if (scope.Resolve<Lazy<ISmallObjectGraphService1>>().Value == null)
            {
                throw new Exception("Null lazy value");
            }

            if (scope.Resolve<Lazy<ISmallObjectGraphService2>>().Value == null)
            {
                throw new Exception("Null lazy value");
            }

            if (scope.Resolve<Lazy<ISmallObjectGraphService3>>().Value == null)
            {
                throw new Exception("Null lazy value");
            }
        }

    }
}