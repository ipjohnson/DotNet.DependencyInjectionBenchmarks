using AF = Autofac;
using G = Grace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Benchmarks.Standard;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Metadata
{
    [BenchmarkCategory("Metadata")]
    public class MetadataBenchmark : BaseBenchmark
    {
        [GlobalSetup]
        public void Setup()
        {
            var definitions = Definitions().ToArray();

            SetupScopeForTest(CreateAutofacScope(), definitions);
            SetupScopeForTest(CreateGraceScope(), definitions);
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
        public void Autofac()
        {
            AutofacScope.Resolve<AF.Features.Metadata.Meta<ISmallObjectGraphService1>>();
            AutofacScope.Resolve<AF.Features.Metadata.Meta<ISmallObjectGraphService2>>();
            AutofacScope.Resolve<AF.Features.Metadata.Meta<ISmallObjectGraphService3>>();
        }

        [Benchmark]
        public void Grace()
        {
            GraceScope.Resolve<G.DependencyInjection.Meta<ISmallObjectGraphService1>>();
            GraceScope.Resolve<G.DependencyInjection.Meta<ISmallObjectGraphService2>>();
            GraceScope.Resolve<G.DependencyInjection.Meta<ISmallObjectGraphService3>>();
        }

        #endregion

    }
}
