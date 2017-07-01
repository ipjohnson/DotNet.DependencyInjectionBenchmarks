using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;

namespace DotNet.DependencyInjectionBenchmarks.Containers
{
    public class AutofacContainer : IContainer
    {
        private Autofac.IContainer _container;
        private readonly ContainerBuilder _builder = new ContainerBuilder();

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _container.Dispose();
        }

        public class AutofacScope : IResolveScope
        {
            private readonly ILifetimeScope _lifetimeScope;

            public AutofacScope(ILifetimeScope lifetimeScope)
            {
                _lifetimeScope = lifetimeScope;
            }

            /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
            public void Dispose()
            {
                _lifetimeScope.Dispose();
            }

            public IResolveScope CreateScope(string scopeName = "")
            {
                return new AutofacScope(_lifetimeScope.BeginLifetimeScope(scopeName));
            }

            public object Resolve(Type type)
            {
                return _lifetimeScope.Resolve(type);
            }

            public object Resolve(Type type, object data)
            {
                var array = data as Array;

                if (array != null)
                {
                    var parameters = new List<Parameter>();

                    foreach (var dataValue in array)
                    {
                        parameters.Add(new TypedParameter(dataValue.GetType(), dataValue));
                    }

                    return _lifetimeScope.Resolve(type, parameters);
                }

                throw new NotSupportedException("Data must be array");
            }

            public bool TryResolve(Type type, object data, out object value)
            {
                return _lifetimeScope.TryResolve(type, out value);
            }
        }

        public IResolveScope CreateScope(string scopeName = "")
        {
            return new AutofacScope(_container.BeginLifetimeScope(scopeName));
        }

        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        public object Resolve(Type type, object data)
        {
            var array = data as Array;

            if (array != null)
            {
                var parameters = new List<Parameter>();

                foreach (var dataValue in array)
                {
                    parameters.Add(new TypedParameter(dataValue.GetType(), dataValue));
                }

                return _container.Resolve(type, parameters);
            }

            throw new NotSupportedException("Data must be array");
        }

        public bool TryResolve(Type type, object data, out object value)
        {
            return _container.TryResolve(type, out value);
        }

        public void BuildContainer()
        {
            _container = _builder.Build();
        }

        public void Registration(IEnumerable<RegistrationDefinition> definitions)
        {
            foreach (var definition in definitions)
            {
                var registration = _builder.RegisterType(definition.ActivationType);

                if (definition.ExportKey == null)
                {
                    registration.As(definition.ExportType);
                }
                else
                {
                    registration.Keyed(definition.ExportKey, definition.ExportType);
                }
                
                switch (definition.RegistrationLifestyle)
                {
                    case RegistrationLifestyle.Singleton:
                        registration.SingleInstance();
                        break;
                    case RegistrationLifestyle.SingletonPerScope:
                        registration.InstancePerLifetimeScope();
                        break;
                    case RegistrationLifestyle.SingletonPerNamedScope:
                        registration.InstancePerMatchingLifetimeScope(definition.LifestyleInformation);
                        break;
                }

                if (definition.Metadata != null)
                {
                    foreach (var kvp in definition.Metadata)
                    {
                        registration.WithMetadata(kvp.Key.ToString(), kvp.Value);
                    }
                }
            }
        }

        public void RegisterFactory<TResult>(Func<TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle)
        {
            var register = _builder.Register(c => factory());

            switch (lifestyle)
            {
                case RegistrationLifestyle.Singleton:
                    register.SingleInstance();
                    break;
                case RegistrationLifestyle.SingletonPerScope:
                    register.InstancePerLifetimeScope();
                    break;
            }
        }

        public void RegisterFactory<T1, TResult>(Func<T1, TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle)
        {
            var register = _builder.Register(c => factory(c.Resolve<T1>()));

            switch (lifestyle)
            {
                case RegistrationLifestyle.Singleton:
                    register.SingleInstance();
                    break;
                case RegistrationLifestyle.SingletonPerScope:
                    register.InstancePerLifetimeScope();
                    break;
            }
        }

        public void RegisterFactory<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle)
        {
            var register = _builder.Register(c => factory(c.Resolve<T1>(), c.Resolve<T2>(), c.Resolve<T3>()));

            switch (lifestyle)
            {
                case RegistrationLifestyle.Singleton:
                    register.SingleInstance();
                    break;
                case RegistrationLifestyle.SingletonPerScope:
                    register.InstancePerLifetimeScope();
                    break;
            }
        }
    }
}
