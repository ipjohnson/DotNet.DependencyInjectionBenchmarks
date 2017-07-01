using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grace.DependencyInjection;
using Grace.Dynamic;

namespace DotNet.DependencyInjectionBenchmarks.Containers
{
    public class GraceContainer : IContainer
    {
        private readonly DependencyInjectionContainer _container = new DependencyInjectionContainer(GraceDynamicMethod.Configuration());

        public void Dispose()
        {
            _container.Dispose();
        }

        public class GraceScope : IResolveScope
        {
            private readonly IExportLocatorScope _scope;

            public GraceScope(IExportLocatorScope scope)
            {
                _scope = scope;
            }

            /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
            public void Dispose()
            {
                _scope.Dispose();
            }

            public IResolveScope CreateScope(string scopeName = "")
            {
                return new GraceScope(_scope.BeginLifetimeScope(scopeName));
            }

            public object Resolve(Type type)
            {
                return _scope.Locate(type);
            }

            public object Resolve(Type type, object data)
            {
                return _scope.Locate(type, data);
            }

            public bool TryResolve(Type type, object data, out object value)
            {
                return _scope.TryLocate(type, out value, data);
            }
        }

        public IResolveScope CreateScope(string scopeName = "")
        {
            return new GraceScope(_container.BeginLifetimeScope(scopeName));
        }

        public object Resolve(Type type)
        {
            return _container.Locate(type);
        }

        public object Resolve(Type type, object data)
        {
            return _container.Locate(type, data);
        }

        public bool TryResolve(Type type, object data, out object value)
        {
            return _container.TryLocate(type, out value, data);
        }

        public void BuildContainer()
        {

        }

        public void Registration(IEnumerable<RegistrationDefinition> definitions)
        {
            _container.Configure(c =>
            {
                foreach (var definition in definitions)
                {
                    var export = c.Export(definition.ActivationType);

                    if (definition.ExportKey == null)
                    {
                        export.As(definition.ExportType);
                    }
                    else
                    {
                        export.AsKeyed(definition.ExportType, definition.ExportKey);
                    }

                    switch (definition.RegistrationLifestyle)
                    {
                        case RegistrationLifestyle.Singleton:
                            export.Lifestyle.Singleton();
                            break;
                        case RegistrationLifestyle.SingletonPerScope:
                            export.Lifestyle.SingletonPerScope();
                            break;
                        case RegistrationLifestyle.SingletonPerObjectGraph:
                            export.Lifestyle.SingletonPerObjectGraph();
                            break;
                        case RegistrationLifestyle.SingletonPerNamedScope:
                            export.Lifestyle.SingletonPerNamedScope(definition.LifestyleInformation as string);
                            break;
                        case RegistrationLifestyle.SingletonPerAncestor:
                            export.Lifestyle.SingletonPerAncestor(definition.LifestyleInformation as Type);
                            break;
                    }

                    if (definition.Metadata != null)
                    {
                        foreach (var kvp in definition.Metadata)
                        {
                            export.WithMetadata(kvp.Key, kvp.Value);
                        }
                    }
                }
            });
        }

        public void RegisterFactory<TResult>(Func<TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle) where TResult : class
        {
            _container.Configure(c =>
            {
                var export = c.ExportFactory(factory);

                switch (lifestyle)
                {
                    case RegistrationLifestyle.Singleton:
                        export.Lifestyle.Singleton();
                        break;
                    case RegistrationLifestyle.SingletonPerScope:
                        export.Lifestyle.SingletonPerScope();
                        break;
                    case RegistrationLifestyle.SingletonPerObjectGraph:
                        export.Lifestyle.SingletonPerObjectGraph();
                        break;
                }
            });
        }

        public void RegisterFactory<T1, TResult>(Func<T1, TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle) where TResult : class
        {
            _container.Configure(c =>
            {
                var export = c.ExportFactory(factory);

                switch (lifestyle)
                {
                    case RegistrationLifestyle.Singleton:
                        export.Lifestyle.Singleton();
                        break;
                    case RegistrationLifestyle.SingletonPerScope:
                        export.Lifestyle.SingletonPerScope();
                        break;
                    case RegistrationLifestyle.SingletonPerObjectGraph:
                        export.Lifestyle.SingletonPerObjectGraph();
                        break;
                }
            });
        }

        public void RegisterFactory<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle) where TResult : class
        {
            _container.Configure(c =>
            {
                var export = c.ExportFactory(factory);

                switch (lifestyle)
                {
                    case RegistrationLifestyle.Singleton:
                        export.Lifestyle.Singleton();
                        break;
                    case RegistrationLifestyle.SingletonPerScope:
                        export.Lifestyle.SingletonPerScope();
                        break;
                    case RegistrationLifestyle.SingletonPerObjectGraph:
                        export.Lifestyle.SingletonPerObjectGraph();
                        break;
                }
            });
        }
    }
}
