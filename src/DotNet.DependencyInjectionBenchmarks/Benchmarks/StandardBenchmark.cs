using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Containers;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks
{
    public abstract class StandardBenchmark : BaseBenchmark
    {
        [GlobalSetup(Target = "Autofac")]
        public void AutofacSetup()
        {
            SetupContainerForTest(CreateAutofacContainer(), Definitions, Warmup);
        }

        [GlobalSetup(Target = "CastleWindsor")]
        public void CastleWindsorSetup()
        {
            SetupContainerForTest(CreateCastleWindsorContainer(), Definitions, Warmup);
        }

        [GlobalSetup(Target = "DryIoc")]
        public void DryIocSetup()
        {
            SetupContainerForTest(CreateDryIocContainer(), Definitions, Warmup);
        }

        [GlobalSetup(Target = "Grace")]
        public void GraceSetup()
        {
            SetupContainerForTest(CreateGraceContainer(), Definitions, Warmup);
        }

        [GlobalSetup(Target = "LightInject")]
        public void LightInjectSetup()
        {
            SetupContainerForTest(CreateLightInjectContainer(), Definitions, Warmup);
        }

        [GlobalSetup(Target = "MicrosoftDependencyInjection")]
        public void MicrosoftDependencyInjectionSetup()
        {
            SetupContainerForTest(CreateMicrosoftDependencyInjectionContainer(), Definitions, Warmup);
        }

        [GlobalSetup(Target = "NInject")]
        public void NInjectSetup()
        {
            SetupContainerForTest(CreateLightInjectContainer(), Definitions, Warmup);
        }

        [GlobalSetup(Target = "SimpleInjector")]
        public void SimpleInjectorSetup()
        {
            SetupContainerForTest(CreateSimpleInjectorContainer(), Definitions, Warmup);
        }

        [GlobalSetup(Target = "StructureMap")]
        public void StructureMapSetup()
        {
            SetupContainerForTest(CreateStructureMapContainer(), Definitions, Warmup);
        }

        protected virtual void Warmup(IResolveScope scope)
        {
            ExecuteBenchmark(scope);
        }

        protected abstract IEnumerable<RegistrationDefinition> Definitions { get; }

        protected abstract void ExecuteBenchmark(IResolveScope scope);
    }
}
