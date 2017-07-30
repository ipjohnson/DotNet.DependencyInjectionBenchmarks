using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;
using System.Linq;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Lookup
{
    [BenchmarkCategory("Lookup")]
    public class LookupBenchmark : BaseBenchmark
    {
        public static string Description =>
            @"This benchmark is designed to test the lookup performance of each container. One small object is resolved from the container along with {ExtraRegistrations} dummy registrations that are located at warmup time.";

        [Params(0, 50, 100, 500, 2000)]
        public int ExtraRegistrations { get; set; }
        
        [GlobalSetup(Target = nameof(Autofac))]
        public void AutofacSetup()
        {
            SetupContainerForLookup(CreateAutofacContainer());
        }

        [Benchmark]
        [BenchmarkCategory(nameof(Autofac))]
        public void Autofac()
        {
            ExecuteBenchmark(AutofacContainer);
        }

        [GlobalSetup(Target = nameof(CastleWindsor))]
        public void CastleWindsorSetup()
        {
            SetupContainerForLookup(CreateCastleWindsorContainer());
        }

        [Benchmark]
        [BenchmarkCategory(nameof(CastleWindsor))]
        public void CastleWindsor()
        {
            ExecuteBenchmark(CastleWindsorContainer);
        }

        [GlobalSetup(Target = nameof(DryIoc))]
        public void DryIocSetup()
        {
            SetupContainerForLookup(CreateDryIocContainer());
        }

        [Benchmark]
        [BenchmarkCategory(nameof(DryIoc))]
        public void DryIoc()
        {
            ExecuteBenchmark(DryIocContainer);
        }

        [GlobalSetup(Target = nameof(Grace))]
        public void GraceSetup()
        {
            SetupContainerForLookup(CreateGraceContainer());
        }

        [Benchmark]
        [BenchmarkCategory(nameof(Grace))]
        public void Grace()
        {
            ExecuteBenchmark(GraceContainer);
        }

        [GlobalSetup(Target = nameof(LightInject))]
        public void LightInjectSetup()
        {
            SetupContainerForLookup(CreateLightInjectContainer());
        }

        [Benchmark]
        [BenchmarkCategory(nameof(LightInject))]
        public void LightInject()
        {
            ExecuteBenchmark(LightInjectContainer);
        }

        [GlobalSetup(Target = nameof(MicrosoftDependencyInjection))]
        public void MicrosoftDependencyInjectionSetup()
        {
            SetupContainerForLookup(CreateMicrosoftDependencyInjectionContainer());
        }

        [Benchmark]
        [BenchmarkCategory(nameof(MicrosoftDependencyInjection))]
        public void MicrosoftDependencyInjection()
        {
            ExecuteBenchmark(MicrosoftDependencyInjectionContainer);
        }

        [GlobalSetup(Target = nameof(NInject))]
        public void NInjectSetup()
        {
            SetupContainerForLookup(CreateLightInjectContainer());
        }

        [Benchmark]
        [BenchmarkCategory(nameof(NInject))]
        public void NInject()
        {
            ExecuteBenchmark(NInjectContainer);
        }

        [GlobalSetup(Target = nameof(SimpleInjector))]
        public void SimpleInjectorSetup()
        {
            SetupContainerForLookup(CreateSimpleInjectorContainer());
        }

        [Benchmark]
        [BenchmarkCategory(nameof(SimpleInjector))]
        public void SimpleInjector()
        {
            ExecuteBenchmark(SimpleInjectorContainer);
        }

        [GlobalSetup(Target = nameof(StructureMap))]
        public void StructureMapSetup()
        {
            SetupContainerForLookup(CreateStructureMapContainer());
        }

        [Benchmark]
        [BenchmarkCategory(nameof(StructureMap))]
        public void StructureMap()
        {
            ExecuteBenchmark(StructureMapContainer);
        }
        
        public void SetupContainerForLookup(IContainer scope)
        {
            var allTypes = DummyClasses.GetTypes(ExtraRegistrations)
                .Select(t => new RegistrationDefinition { ExportType = t, ActivationType = t }).ToList();

            var definitions = SmallObjectServices.Definitions().ToArray();

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

        private void ExecuteBenchmark(IResolveScope scope)
        {
            scope.Resolve(typeof(ISmallObjectService));
        }

    }
}
