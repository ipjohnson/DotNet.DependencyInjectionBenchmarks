using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LightInject;

namespace DotNet.DependencyInjectionBenchmarks.Containers
{
    public class LightInjectContainer : IContainer
    {
        private readonly ServiceContainer _container = new ServiceContainer();

        public string DisplayName => "LightInject";

        public string Version => typeof(ServiceContainer).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "1.0.0";

        public string WebSite => "http://www.lightinject.net/";
        
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _container.Dispose();
        }

        public class LightInjectScope : IResolveScope
        {
            private Scope _scope;

            public LightInjectScope(Scope scope)
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
                return new LightInjectScope(_scope.BeginScope());
            }

            public object Resolve(Type type)
            {
                return _scope.GetInstance(type);
            }

            public object Resolve(Type type, object data)
            {
                var array = data as object[];

                if (array != null)
                {
                    return _scope.GetInstance(type, array);
                }

                throw new NotSupportedException("data must be array");
            }

            public bool TryResolve(Type type, object data, out object value)
            {
                return (value = _scope.TryGetInstance(type)) != null;
            }
        }

        public IResolveScope CreateScope(string scopeName = "")
        {
            var scope = _container.BeginScope();

            return new LightInjectScope(scope);
        }

        public object Resolve(Type type)
        {
            return _container.GetInstance(type);
        }

        public object Resolve(Type type, object data)
        {
            var array = data as object[];

            if (array != null)
            {
                return _container.GetInstance(type, array);
            }

            throw new NotSupportedException("data must be array");
        }

        public bool TryResolve(Type type, object data, out object value)
        {
            return (value = _container.TryGetInstance(type)) != null;
        }

        public void BuildContainer()
        {

        }

        public void Registration(IEnumerable<RegistrationDefinition> definitions)
        {
            foreach (var definition in definitions)
            {
                ILifetime lifetime = null;

                switch (definition.RegistrationLifestyle)
                {
                    case RegistrationLifestyle.Singleton:
                        lifetime = new PerContainerLifetime();
                        break;
                    case RegistrationLifestyle.SingletonPerScope:
                        lifetime = new PerScopeLifetime();
                        break;
                }

                if (definition.RegistrationMode == RegistrationMode.Single)
                {
                    _container.Register(definition.ExportType, definition.ActivationType, lifetime);
                }
                else
                {
                    _container.Register(definition.ExportType, definition.ActivationType, definition.ActivationType.Name, lifetime);
                }
            }
        }

        public void RegisterFactory<TResult>(Func<TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle) where TResult : class
        {
            var registration = _container.Register(serviceFactory => factory());

            switch (lifestyle)
            {
                case RegistrationLifestyle.Singleton:
                    registration.SetDefaultLifetime<PerContainerLifetime>();
                    break;
                case RegistrationLifestyle.SingletonPerScope:
                    registration.SetDefaultLifetime<PerScopeLifetime>();
                    break;
            }
        }

        public void RegisterFactory<T1, TResult>(Func<T1, TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle) where TResult : class
        {
            var registration = _container.Register<T1, TResult>((serviceFactory, arg1) => factory(arg1));

            switch (lifestyle)
            {
                case RegistrationLifestyle.Singleton:
                    registration.SetDefaultLifetime<PerContainerLifetime>();
                    break;
                case RegistrationLifestyle.SingletonPerScope:
                    registration.SetDefaultLifetime<PerScopeLifetime>();
                    break;
            }
        }

        public void RegisterFactory<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle) where TResult : class
        {
            var registration = _container.Register<T1, T2, T3, TResult>((serviceFactory, arg1, arg2, arg3) => factory(arg1, arg2, arg3));

            switch (lifestyle)
            {
                case RegistrationLifestyle.Singleton:
                    registration.SetDefaultLifetime<PerContainerLifetime>();
                    break;
                case RegistrationLifestyle.SingletonPerScope:
                    registration.SetDefaultLifetime<PerScopeLifetime>();
                    break;
            }
        }
    }
}
