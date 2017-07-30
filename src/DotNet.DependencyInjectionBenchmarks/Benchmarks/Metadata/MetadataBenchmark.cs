using AF = Autofac;
using G = Grace;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Metadata
{
    [BenchmarkCategory("Metadata")]
    public class MetadataBenchmark : BaseBenchmark
    {
        public static string Description =>
            "This benchmark registers types with metadata then resolves a untyped metadata model along side the desired service.";

        [GlobalSetup]
        public void Setup()
        {
            var definitions = Definitions().ToArray();

            SetupContainerForTest(CreateAutofacContainer(), definitions);
            SetupContainerForTest(CreateGraceContainer(), definitions);
        }

        public static IEnumerable<RegistrationDefinition> Definitions()
        {
            var metadata = new Dictionary<object, object>
            {
                { "IntProp", 5},
                { "DoubleProp", 5.0 },
                { "StringProp", "StringValue1" }
            };

            yield return new RegistrationDefinition { ExportType = typeof(ISmallObjectService), ActivationType = typeof(SmallObjectService), Metadata = metadata };
            yield return new RegistrationDefinition { ExportType = typeof(ITransientService), ActivationType = typeof(TransientService) };
            yield return new RegistrationDefinition { ExportType = typeof(ISingletonService), ActivationType = typeof(SingletonService), RegistrationLifestyle = RegistrationLifestyle.Singleton };

        }

        #region Benchmarks

        [Benchmark]
        [BenchmarkCategory(nameof(Autofac))]
        public void Autofac()
        {
            AutofacContainer.Resolve<AF.Features.Metadata.Meta<ISmallObjectService>>();
        }

        [Benchmark]
        [BenchmarkCategory(nameof(Grace))]
        public void Grace()
        {
            GraceContainer.Resolve<G.DependencyInjection.Meta<ISmallObjectService>>();
        }

        #endregion

    }
}
