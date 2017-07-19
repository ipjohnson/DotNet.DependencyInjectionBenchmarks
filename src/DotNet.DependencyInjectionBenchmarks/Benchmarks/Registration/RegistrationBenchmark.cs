using System;
using System.Linq;
using Autofac;
using BenchmarkDotNet.Attributes;
using DotNet.DependencyInjectionBenchmarks.Classes;
using DryIoc;
using Grace.DependencyInjection;
using Grace.Dynamic;
using Microsoft.Extensions.DependencyInjection;
using Ninject;

namespace DotNet.DependencyInjectionBenchmarks.Benchmarks.Registration
{
    public enum ResolveScenario
    {
        ResolveNone,
        ResolveOne,
        ResolveHalf,
        ResolveAll
    }

    [BenchmarkCategory("Registration")]
    public class RegistrationBenchmark
    {
        private Type[] _types;

        [Params(10, 100, 500)]
        public int Registrations { get; set; }

        [Params(ResolveScenario.ResolveNone, ResolveScenario.ResolveOne, ResolveScenario.ResolveHalf, ResolveScenario.ResolveAll)]
        public ResolveScenario Scenario { get; set; }
        
        public static string Description =>
            "This benchmark registers {Registrations} classes then {Scenario} from the container";

        [GlobalSetup]
        public void Setup()
        {
            _types = DummyClasses.GetTypes(Registrations).ToArray();
        }

        [Benchmark]
        [BenchmarkCategory("Autofac")]
        public void Autofac()
        {
            var containerBuilder = new ContainerBuilder();

            foreach (var type in _types)
            {
                containerBuilder.RegisterType(type).As(type);
            }

            var container = containerBuilder.Build();
            int length = 0;

            if (Scenario == ResolveScenario.ResolveOne)
            {
                length = 1;
            }
            else if (Scenario == ResolveScenario.ResolveHalf)
            {
                length = _types.Length / 2;
            }
            else if (Scenario == ResolveScenario.ResolveAll)
            {
                length = _types.Length;
            }

            for (var i = 0; i < length; i++)
            {
                container.Resolve(_types[i]);
            }

            container.Dispose();
        }

        [Benchmark]
        [BenchmarkCategory("DryIoc")]
        public void DryIoc()
        {
            var container = new DryIoc.Container();

            foreach (var type in _types)
            {
                container.Register(type, type);
            }
            int length = 0;

            if (Scenario == ResolveScenario.ResolveOne)
            {
                length = 1;
            }
            else if (Scenario == ResolveScenario.ResolveHalf)
            {
                length = _types.Length / 2;
            }
            else if (Scenario == ResolveScenario.ResolveAll)
            {
                length = _types.Length;
            }

            for (var i = 0; i < length; i++)
            {
                container.Resolve(_types[i]);
            }

            container.Dispose();
        }

        [Benchmark]
        [BenchmarkCategory("Grace")]
        public void Grace()
        {
            var container = new DependencyInjectionContainer(GraceDynamicMethod.Configuration());

            container.Configure(c =>
            {
                foreach (var type in _types)
                {
                    c.Export(type).As(type);
                }
            });

            int length = 0;

            if (Scenario == ResolveScenario.ResolveOne)
            {
                length = 1;
            }
            else if (Scenario == ResolveScenario.ResolveHalf)
            {
                length = _types.Length / 2;
            }
            else if (Scenario == ResolveScenario.ResolveAll)
            {
                length = _types.Length;
            }

            for (var i = 0; i < length; i++)
            {
                container.Locate(_types[i]);
            }

            container.Dispose();
        }

        [Benchmark]
        [BenchmarkCategory("LightInject")]
        public void LightInject()
        {
            var container = new LightInject.ServiceContainer();

            foreach (var type in _types)
            {
                container.Register(type, type);
            }

            int length = 0;

            if (Scenario == ResolveScenario.ResolveOne)
            {
                length = 1;
            }
            else if (Scenario == ResolveScenario.ResolveHalf)
            {
                length = _types.Length / 2;
            }
            else if (Scenario == ResolveScenario.ResolveAll)
            {
                length = _types.Length;
            }

            for (var i = 0; i < length; i++)
            {
                container.GetInstance(_types[i]);
            }

            container.Dispose();
        }

        [Benchmark]
        [BenchmarkCategory("MicrosoftDependencyInjection")]
        public void MicrosoftDependencyInjection()
        {
            var serviceCollection = new ServiceCollection();

            foreach (var type in _types)
            {
                serviceCollection.AddTransient(type, type);
            }

            var container = serviceCollection.BuildServiceProvider();

            int length = 0;

            if (Scenario == ResolveScenario.ResolveOne)
            {
                length = 1;
            }
            else if (Scenario == ResolveScenario.ResolveHalf)
            {
                length = _types.Length / 2;
            }
            else if (Scenario == ResolveScenario.ResolveAll)
            {
                length = _types.Length;
            }

            for (var i = 0; i < length; i++)
            {
                container.GetService(_types[i]);
            }

            ((IDisposable)container).Dispose();
        }

        [Benchmark]
        [BenchmarkCategory("NInject")]
        public void NInject()
        {
            var configuration = new KernelConfiguration();

            foreach (var type in _types)
            {
                configuration.Bind(type).To(type);
            }

            var container = configuration.BuildReadonlyKernel();

            int length = 0;

            if (Scenario == ResolveScenario.ResolveOne)
            {
                length = 1;
            }
            else if (Scenario == ResolveScenario.ResolveHalf)
            {
                length = _types.Length / 2;
            }
            else if (Scenario == ResolveScenario.ResolveAll)
            {
                length = _types.Length;
            }

            for (var i = 0; i < length; i++)
            {
                container.GetService(_types[i]);
            }

            container.Dispose();
        }

        [Benchmark]
        [BenchmarkCategory("SimpleInjector")]
        public void SimpleInjector()
        {
            var container = new SimpleInjector.Container();

            foreach (var type in _types)
            {
                container.Register(type, type);
            }

            int length = 0;

            if (Scenario == ResolveScenario.ResolveOne)
            {
                length = 1;
            }
            else if (Scenario == ResolveScenario.ResolveHalf)
            {
                length = _types.Length / 2;
            }
            else if (Scenario == ResolveScenario.ResolveAll)
            {
                length = _types.Length;
            }

            for (var i = 0; i < length; i++)
            {
                container.GetInstance(_types[i]);
            }

            container.Dispose();
        }

        [Benchmark]
        [BenchmarkCategory("StructureMap")]
        public void StructureMap()
        {
            var container = new StructureMap.Container();

            foreach (var type in _types)
            {
                container.Configure(c => c.For(type).Use(type));
            }

            int length = 0;

            if (Scenario == ResolveScenario.ResolveOne)
            {
                length = 1;
            }
            else if (Scenario == ResolveScenario.ResolveHalf)
            {
                length = _types.Length / 2;
            }
            else if (Scenario == ResolveScenario.ResolveAll)
            {
                length = _types.Length;
            }

            for (var i = 0; i < length; i++)
            {
                container.GetInstance(_types[i]);
            }

            container.Dispose();
        }
    }
}
