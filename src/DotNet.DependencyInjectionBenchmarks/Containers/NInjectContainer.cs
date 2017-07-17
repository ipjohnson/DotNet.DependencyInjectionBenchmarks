using System;
using System.Collections.Generic;
using System.Reflection;
using Ninject;

namespace DotNet.DependencyInjectionBenchmarks.Containers
{
    public class NInjectContainer : IContainer
    {
        private IReadOnlyKernel _kernel;
        private readonly KernelConfiguration _configuration = new KernelConfiguration();

        public string DisplayName => "NInject";

        public string Version => typeof(IReadOnlyKernel).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "1.0.0";

        public string WebSite => "http://www.ninject.org/";

        public void Dispose()
        {
            _kernel.Dispose();
        }
        
        public IResolveScope CreateScope(string scopeName = "")
        {
            // ninject 4.0 child scope and named scopes aren't supported yet
            throw new NotSupportedException();
        }

        public object Resolve(Type type)
        {
            return _kernel.Get(type);
        }

        public object Resolve(Type type, object data)
        {
            throw new NotImplementedException();
        }

        public bool TryResolve(Type type, object data, out object value)
        {
            value = _kernel.TryGet(type);

            return value != null;
        }

        public void BuildContainer()
        {
            _kernel = _configuration.BuildReadonlyKernel();
        }

        public void Registration(IEnumerable<RegistrationDefinition> definitions)
        {
            foreach (var definition in definitions)
            {
                var binding = _configuration.Bind(definition.ExportType).To(definition.ActivationType);

                switch (definition.RegistrationLifestyle)
                {
                    case RegistrationLifestyle.Singleton:
                        binding.InSingletonScope();
                        break;
                    case RegistrationLifestyle.Transient:
                        binding.InTransientScope();
                        break;
                    default:
                        throw new NotSupportedException();
                }

                if (definition.Metadata != null)
                {
                    foreach (var pair in definition.Metadata)
                    {
                        binding.BindingConfiguration.Metadata.Set(pair.Key.ToString(), pair.Value);
                    }
                }
            }
        }

        public void RegisterFactory<TResult>(Func<TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle) where TResult : class
        {
            var binding = _configuration.Bind<TResult>().ToMethod(context => factory());
            
            switch (lifestyle)
            {
                case RegistrationLifestyle.Singleton:
                    binding.InSingletonScope();
                    break;
                case RegistrationLifestyle.Transient:
                    binding.InTransientScope();
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public void RegisterFactory<T1, TResult>(Func<T1, TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle) where TResult : class
        {
            var binding = _configuration.Bind<TResult>().ToMethod(context => factory(context.Kernel.Get<T1>()));

            switch (lifestyle)
            {
                case RegistrationLifestyle.Singleton:
                    binding.InSingletonScope();
                    break;
                case RegistrationLifestyle.Transient:
                    binding.InTransientScope();
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public void RegisterFactory<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle) where TResult : class
        {
            var binding = _configuration.Bind<TResult>().ToMethod(context => factory(context.Kernel.Get<T1>(), context.Kernel.Get<T2>(), context.Kernel.Get<T3>()));

            switch (lifestyle)
            {
                case RegistrationLifestyle.Singleton:
                    binding.InSingletonScope();
                    break;
                case RegistrationLifestyle.Transient:
                    binding.InTransientScope();
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
