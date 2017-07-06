using AF = Autofac;
using G = Grace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Metadata
{
    [BenchmarkCategory("Metadata")]
    public class StronglyTypedMetadataBenchmark : BaseBenchmark
    {
        public static string Description =>
            "This benchmark registers types with metadata then resolves a strongly type metadata model along side the desired service.";

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

            yield return new RegistrationDefinition { ExportType = typeof(ISmallObjectGraphService1), ActivationType = typeof(SmallObjectGraphService1), Metadata = metadata };
            yield return new RegistrationDefinition { ExportType = typeof(ITransientService1), ActivationType = typeof(TransientService1) };

            metadata = new Dictionary<object, object>
            {
                { "IntProp", 10 },
                { "DoubleProp", 10.0 },
                { "StringProp", "StringValue2" }
            };

            yield return new RegistrationDefinition { ExportType = typeof(ISmallObjectGraphService2), ActivationType = typeof(SmallObjectGraphService2), Metadata = metadata };
            yield return new RegistrationDefinition { ExportType = typeof(ITransientService2), ActivationType = typeof(TransientService2) };

            metadata = new Dictionary<object, object>
            {
                { "IntProp", 15 },
                { "DoubleProp", 15.0 },
                { "StringProp", "StringValue3" }
            };

            yield return new RegistrationDefinition { ExportType = typeof(ISmallObjectGraphService3), ActivationType = typeof(SmallObjectGraphService3), Metadata = metadata };
            yield return new RegistrationDefinition { ExportType = typeof(ITransientService3), ActivationType = typeof(TransientService3) };

            foreach (var definition in SingletonBenchmark.Definitions())
            {
                yield return definition;
            }
        }

        #region Benchmarks

        [Benchmark]
        [BenchmarkCategory("Autofac")]
        public void Autofac()
        {
            AutofacContainer.Resolve<AF.Features.Metadata.Meta<ISmallObjectGraphService1, MetadataClass>>();
            AutofacContainer.Resolve<AF.Features.Metadata.Meta<ISmallObjectGraphService2, MetadataClass>>();
            AutofacContainer.Resolve<AF.Features.Metadata.Meta<ISmallObjectGraphService3, MetadataClass>>();
        }

        [Benchmark]
        [BenchmarkCategory("Autofac")]
        public void Grace()
        {
            GraceContainer.Resolve<G.DependencyInjection.Meta<ISmallObjectGraphService1, MetadataClass>>();
            GraceContainer.Resolve<G.DependencyInjection.Meta<ISmallObjectGraphService2, MetadataClass>>();
            GraceContainer.Resolve<G.DependencyInjection.Meta<ISmallObjectGraphService3, MetadataClass>>();
        }

        #endregion
    }
}
