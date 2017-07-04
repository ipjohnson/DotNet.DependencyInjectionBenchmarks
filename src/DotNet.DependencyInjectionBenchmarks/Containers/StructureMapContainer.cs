using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using StructureMap;
using StructureMap.Pipeline;

namespace DotNet.DependencyInjectionBenchmarks.Containers
{
    public class StructureMapContainer : IContainer
    {
        private StructureMap.Container _container = new Container();

        public string DisplayName => "StructureMap";

        public string Version => typeof(Container).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "1.0.0";

        public string WebSite => "http://structuremap.github.io/";


        public void BuildContainer()
        {

        }

        public class StructureMapChildScope : IResolveScope
        {
            private StructureMap.IContainer _container;

            public StructureMapChildScope(StructureMap.IContainer container)
            {
                _container = container;
            }

            /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
            public void Dispose()
            {
                _container.Dispose();
            }

            public IResolveScope CreateScope(string scopeName = "")
            {
                return new StructureMapChildScope(_container.CreateChildContainer());
            }

            public object Resolve(Type type)
            {
                return _container.GetInstance(type);
            }

            public object Resolve(Type type, object data)
            {
                if (data is IDictionary<string, object>)
                {
                    return _container.GetInstance(type, new ExplicitArguments((IDictionary<string, object>)data));
                }

                throw new NotSupportedException();
            }

            public bool TryResolve(Type type, object data, out object value)
            {
                if (data is null)
                {
                    value = _container.TryGetInstance(type);

                    return value != null;
                }

                if (data is IDictionary<string, object>)
                {
                    value = _container.TryGetInstance(type, new ExplicitArguments((IDictionary<string, object>)data));

                    return value != null;
                }

                throw new NotSupportedException();
            }
        }

        public IResolveScope CreateScope(string scopeName = "")
        {
            return new StructureMapChildScope(_container.CreateChildContainer());
        }

        public void Dispose()
        {
            _container.Dispose();
        }

        public void RegisterFactory<TResult>(Func<TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle) where TResult : class
        {
            var lifecycle = GetLifecycle(lifestyle);

            _container.Configure(r => r.For<TResult>(lifecycle).Use(() => factory()));
        }

        public void RegisterFactory<T1, TResult>(Func<T1, TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle) where TResult : class
        {
            var lifecycle = GetLifecycle(lifestyle);

            _container.Configure(r => r.For<TResult>(lifecycle).Use(context => factory(context.GetInstance<T1>())));
        }

        public void RegisterFactory<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle) where TResult : class
        {
            var lifecycle = GetLifecycle(lifestyle);

            _container.Configure(r => r.For<TResult>(lifecycle).Use(context => factory(context.GetInstance<T1>(), context.GetInstance<T2>(), context.GetInstance<T3>())));
        }

        public void Registration(IEnumerable<RegistrationDefinition> definitions)
        {
            _container.Configure(r =>
            {
                foreach (var definition in definitions)
                {
                    var lifecycle = GetLifecycle(definition.RegistrationLifestyle);

                    r.For(definition.ExportType, lifecycle).Use(definition.ActivationType);
                }
            });
        }

        private ILifecycle GetLifecycle(RegistrationLifestyle lifestyle)
        {
            ILifecycle lifecycle = null;

            switch (lifestyle)
            {
                case RegistrationLifestyle.Singleton:
                    lifecycle = new SingletonLifecycle();
                    break;
                case RegistrationLifestyle.SingletonPerScope:
                    lifecycle = new ContainerLifecycle();
                    break;
            }

            return lifecycle;
        }

        public object Resolve(Type type)
        {
            return _container.GetInstance(type);
        }

        public object Resolve(Type type, object data)
        {
            if (data is IDictionary<string, object>)
            {
                return _container.GetInstance(type, new ExplicitArguments((IDictionary<string, object>)data));
            }

            throw new NotSupportedException();
        }

        public bool TryResolve(Type type, object data, out object value)
        {
            if (data is null)
            {
                value = _container.TryGetInstance(type);

                return value != null;
            }

            if (data is IDictionary<string, object>)
            {
                value = _container.TryGetInstance(type, new ExplicitArguments((IDictionary<string, object>)data));

                return value != null;
            }

            throw new NotSupportedException();
        }
    }
}
