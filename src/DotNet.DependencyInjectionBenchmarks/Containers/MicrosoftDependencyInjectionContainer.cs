using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.DependencyInjectionBenchmarks.Containers
{
    public class MicrosoftDependencyInjectionContainer : IContainer
    {
        private IServiceProvider _serviceProvider;
        private readonly IServiceCollection _serviceCollection = new ServiceCollection();

        public void BuildContainer()
        {
            _serviceProvider = _serviceCollection.BuildServiceProvider();
        }

        public class ChildScope : IResolveScope
        {
            private IServiceProvider _serviceProvider;
            private IServiceScope _serviceScope;

            public ChildScope(IServiceScope serviceScope)
            {
                _serviceScope = serviceScope;
                _serviceProvider = serviceScope.ServiceProvider;
            }

            /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
            public void Dispose()
            {
                _serviceScope.Dispose();
            }

            public IResolveScope CreateScope(string scopeName = "")
            {
                var scopeFactory = _serviceProvider.CreateScope();

                return new ChildScope(scopeFactory);
            }

            public object Resolve(Type type)
            {
                return _serviceProvider.GetService(type);
            }

            public object Resolve(Type type, object data)
            {
                throw new NotImplementedException();
            }

            public bool TryResolve(Type type, object data, out object value)
            {
                return (value = _serviceProvider.GetService(type)) != null;
            }
        }

        public IResolveScope CreateScope(string scopeName = "")
        {
            return new ChildScope(_serviceProvider.CreateScope());
        }

        public void Dispose()
        {
            ((IDisposable)_serviceProvider).Dispose();
        }

        public void RegisterFactory<TResult>(Func<TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle) where TResult : class
        {
            switch (lifestyle)
            {
                case RegistrationLifestyle.Transient:
                    _serviceCollection.AddTransient(provider => factory());
                    break;
                case RegistrationLifestyle.Singleton:
                    _serviceCollection.AddSingleton(provider => factory());
                    break;
                case RegistrationLifestyle.SingletonPerScope:
                    _serviceCollection.AddScoped(provider => factory());
                    break;
            }
        }

        public void RegisterFactory<T1, TResult>(Func<T1, TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle) where TResult : class
        {
            switch (lifestyle)
            {
                case RegistrationLifestyle.Transient:
                    _serviceCollection.AddTransient(provider => factory(provider.GetService<T1>()));
                    break;
                case RegistrationLifestyle.Singleton:
                    _serviceCollection.AddSingleton(provider => factory(provider.GetService<T1>()));
                    break;
                case RegistrationLifestyle.SingletonPerScope:
                    _serviceCollection.AddScoped(provider => factory(provider.GetService<T1>()));
                    break;
            }
        }

        public void RegisterFactory<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle) where TResult : class
        {
            switch (lifestyle)
            {
                case RegistrationLifestyle.Transient:
                    _serviceCollection.AddTransient(provider => factory(provider.GetService<T1>(), provider.GetService<T2>(), provider.GetService<T3>()));
                    break;
                case RegistrationLifestyle.Singleton:
                    _serviceCollection.AddSingleton(provider => factory(provider.GetService<T1>(), provider.GetService<T2>(), provider.GetService<T3>()));
                    break;
                case RegistrationLifestyle.SingletonPerScope:
                    _serviceCollection.AddScoped(provider => factory(provider.GetService<T1>(), provider.GetService<T2>(), provider.GetService<T3>()));
                    break;
            }
        }

        public void Registration(IEnumerable<RegistrationDefinition> definitions)
        {
            foreach (var definition in definitions)
            {
                switch (definition.RegistrationLifestyle)
                {
                    case RegistrationLifestyle.Singleton:
                        _serviceCollection.AddSingleton(definition.ExportType, definition.ActivationType);
                        break;
                    case RegistrationLifestyle.SingletonPerScope:
                        _serviceCollection.AddScoped(definition.ExportType, definition.ActivationType);
                        break;
                    case RegistrationLifestyle.Transient:
                        _serviceCollection.AddTransient(definition.ExportType, definition.ActivationType);
                        break;
                }
            }
        }

        public object Resolve(Type type)
        {
            return _serviceProvider.GetService(type);
        }

        public object Resolve(Type type, object data)
        {
            throw new NotImplementedException();
        }

        public bool TryResolve(Type type, object data, out object value)
        {
            return (value = _serviceProvider.GetService(type)) != null;
        }
    }
}
