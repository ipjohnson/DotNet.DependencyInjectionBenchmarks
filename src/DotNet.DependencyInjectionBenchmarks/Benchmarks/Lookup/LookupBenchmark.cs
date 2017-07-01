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
    public enum ScenarioType
    {
        BestCase,
        AverageCase,
        WorstCase
    }

    [BenchmarkCategory("Lookup")]
    public class LookupBenchmark : BaseBenchmark
    {
        [Params(0, 100)]
        public int ExtraRegistrations { get; set; }

        [Params(ScenarioType.BestCase, ScenarioType.AverageCase, ScenarioType.WorstCase)]
        public ScenarioType Scenario { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            SetupContainer(CreateAutofacContainer());
            SetupContainer(CreateGraceContainer());
            SetupContainer(CreateDryIocContainer());
            SetupContainer(CreateLightInjectContainer());
            SetupContainer(CreateMicrosoftDependencyInjectionContainer());
            SetupContainer(CreateNInjectContainer());
            SetupContainer(CreateSimpleInjectorContainer());
            SetupContainer(CreateStructureMapContainer());
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
            scope.Resolve(typeof(ISmallObjectGraphService2));
            scope.Resolve(typeof(ISmallObjectGraphService3));
        }

        #endregion

        #region Setup Container 

        public void SetupContainer(IContainer scope)
        {
            var allTypes = DummyClasses.GetTypes(ExtraRegistrations)
                .Select(t => new RegistrationDefinition { ExportType = t, ActivationType = t }).ToList();

            if (Scenario == ScenarioType.WorstCase)
            {
                allTypes.InsertRange(0, SmallObjectBenchmark.Definitions());
            }
            else if (Scenario == ScenarioType.AverageCase)
            {
                var definitions = SmallObjectBenchmark.Definitions().ToArray();
                var index = 0;
                var gap = allTypes.Count / definitions.Length;

                foreach (var definition in definitions)
                {
                    allTypes.Insert(index, definition);

                    index += gap + 1;
                }
            }
            else
            {
                allTypes.AddRange(SmallObjectBenchmark.Definitions());
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
