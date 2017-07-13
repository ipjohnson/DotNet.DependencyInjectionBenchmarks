using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace DotNet.DependencyInjectionBenchmarks.Containers
{
    public class CastleWindsorContainer : IContainer
    {
        private WindsorContainer _container = new WindsorContainer();

        public string DisplayName => "Castle Windsor";

        public string Version => typeof(WindsorContainer).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "1.0.0";

        public string WebSite => "https://github.com/castleproject/Windsor";

        public void BuildContainer()
        {

        }

        public IResolveScope CreateScope(string scopeName = "")
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _container.Dispose();
        }

        public void RegisterFactory<TResult>(Func<TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle) where TResult : class
        {
            throw new NotImplementedException();
        }

        public void RegisterFactory<T1, TResult>(Func<T1, TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle) where TResult : class
        {
            throw new NotImplementedException();
        }

        public void RegisterFactory<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle) where TResult : class
        {
            throw new NotImplementedException();
        }

        public void Registration(IEnumerable<RegistrationDefinition> definitions)
        {
            foreach (var definition in definitions)
            {
                switch (definition.RegistrationLifestyle)
                {
                    case RegistrationLifestyle.Singleton:
                        _container.Register(Component.For(definition.ExportType).ImplementedBy(definition.ActivationType)
                            .LifeStyle.Singleton);
                        break;
                    case RegistrationLifestyle.SingletonPerScope:
                        _container.Register(Component.For(definition.ExportType).ImplementedBy(definition.ActivationType)
                            .LifeStyle.Scoped());
                        break;
                    case RegistrationLifestyle.Transient:
                        _container.Register(Component.For(definition.ExportType).ImplementedBy(definition.ActivationType)
                            .LifeStyle.Transient);
                        break;
                }
            }
        }

        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        public object Resolve(Type type, object data)
        {
            return _container.Resolve(type, data);
        }

        public bool TryResolve(Type type, object data, out object value)
        {
            throw new NotImplementedException();
        }
    }
}
