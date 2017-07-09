using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Lookup
{
    [BenchmarkCategory("Lookup")]
    public class LookupBenchmark : BaseBenchmark
    {
        public static string Description =>
            @"This benchmark is designed to test the lookup performance of each container. One small object is resolved from the container along with {ExtraRegistrations} dummy registrations that are located at warmup time.";

        [Params(0, 50, 100, 500, 2000)]
        public int ExtraRegistrations { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            SetupContainerForLookup(CreateAutofacContainer());
            SetupContainerForLookup(CreateGraceContainer());
            SetupContainerForLookup(CreateDryIocContainer());
            SetupContainerForLookup(CreateLightInjectContainer());
            SetupContainerForLookup(CreateMicrosoftDependencyInjectionContainer());
            SetupContainerForLookup(CreateNInjectContainer());
            SetupContainerForLookup(CreateSimpleInjectorContainer());
            SetupContainerForLookup(CreateStructureMapContainer());
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
        [BenchmarkCategory("MicrosoftDependencyInjection")]
        public void MicrosoftDependencyInjection()
        {
            ExecuteBenchmark(MicrosoftDependencyInjectionContainer);
        }

        [Benchmark]
        [BenchmarkCategory("NInject")]
        public void NInject()
        {
            ExecuteBenchmark(NInjectContainer);
        }

        [Benchmark]
        [BenchmarkCategory("SimpleInjector")]
        public void SimpleInjector()
        {
            ExecuteBenchmark(SimpleInjectorContainer);
        }

        [Benchmark]
        [BenchmarkCategory("StructureMap")]
        public void StructureMap()
        {
            ExecuteBenchmark(StructureMapContainer);
        }

        private void ExecuteBenchmark(IResolveScope scope)
        {
            scope.Resolve(typeof(ISmallObjectGraphService1));
        }

        #endregion

        #region Setup Container 

        public void SetupContainerForLookup(IContainer scope)
        {
            var allTypes = DummyClasses.GetTypes(ExtraRegistrations)
                .Select(t => new RegistrationDefinition { ExportType = t, ActivationType = t }).ToList();

            var definitions = SmallObjectBenchmark.Definitions().ToArray();

            var gap = allTypes.Count / definitions.Length;

            var index = gap / 2;

            foreach (var definition in definitions)
            {
                allTypes.Insert(index, definition);

                index += gap + 1;
            }

            scope.Registration(allTypes);

            scope.BuildContainer();

            foreach (var type in allTypes.Select(r => r.ExportType))
            {
                scope.Resolve(type);
            }
        }
        #endregion
    }
}
