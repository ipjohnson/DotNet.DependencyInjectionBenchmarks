using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using SimpleInjector;

namespace DotNet.DependencyInjectionBenchmarks.Containers
{
    public class SimpleInjectorContainerScope : IContainerScope
    {
        private SimpleInjector.Container _container = new Container();

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

        public object Resolve(Type type)
        {
            return _container.GetInstance(type);
        }

        public object Resolve(Type type, object data)
        {
            throw new NotImplementedException();
        }

        public bool TryResolve(Type type, object data, out object value)
        {
            var instances = _container.GetAllInstances(type);

            value = instances.FirstOrDefault();

            return value != null;
        }

        public void RegisterFactory<TResult>(Func<TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle)
        {
            throw new NotImplementedException();
        }

        public void RegisterFactory<T1, TResult>(Func<T1, TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle)
        {
            throw new NotImplementedException();
        }

        public void RegisterFactory<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle)
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
                        _container.RegisterSingleton(definition.ExportType, definition.ActivationType);
                        break;
                    case RegistrationLifestyle.Transient:
                        _container.Register(definition.ExportType, definition.ActivationType);
                        break;
                }
            }
        }
    }
}
