using System;
using System.Collections.Generic;
using System.Reflection;
using DryIoc;

namespace DotNet.DependencyInjectionBenchmarks.Containers
{
    public class DryIocContainer : IContainer
    {
        private readonly Container _container = new Container(rules => rules.WithImplicitRootOpenScope());

        public string DisplayName => "DryIoc";

        public string Version => typeof(Container).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "1.0.0";

        public string WebSite => "https://bitbucket.org/dadhi/dryioc";


        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _container.Dispose();
        }

        public class DryIocScope : IResolveScope
        {
            private readonly DryIoc.IContainer _scope;

            public DryIocScope(DryIoc.IContainer scope)
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
                var newScope = _scope.OpenScope(scopeName);

                return new DryIocScope(newScope);
            }

            public object Resolve(Type type)
            {
                return _scope.Resolve(type);
            }

            public object Resolve(Type type, object data)
            {
                throw new NotImplementedException();
            }

            public bool TryResolve(Type type, object data, out object value)
            {
                value = _scope.Resolve(type, IfUnresolved.ReturnDefault);

                return value != null;
            }
        }

        public IResolveScope CreateScope(string scopeName = "")
        {
            var scope = _container.OpenScope();

            return new DryIocScope(scope);
        }

        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        public object Resolve(Type type, object data)
        {
            throw new NotImplementedException();
        }

        public bool TryResolve(Type type, object data, out object value)
        {
            value = _container.Resolve(type, IfUnresolved.ReturnDefault);

            return value != null;
        }

        public void BuildContainer()
        {

        }

        public void Registration(IEnumerable<RegistrationDefinition> definitions)
        {
            foreach (var definition in definitions)
            {
                IReuse reuse = null;

                switch (definition.RegistrationLifestyle)
                {
                    case RegistrationLifestyle.Singleton:
                        reuse = new SingletonReuse();
                        break;
                    case RegistrationLifestyle.SingletonPerScope:
                        reuse = new CurrentScopeReuse();
                        break;
                }

                PropertiesAndFieldsSelector madeOf = null;

                if (definition.MemberInjectionList != null)
                {
                    madeOf = PropertiesAndFields.Of;

                    foreach (var injectionInfo in definition.MemberInjectionList)
                    {
                        switch (injectionInfo.InjectionType)
                        {
                            case MemberInjectionType.Field:
                            case MemberInjectionType.Property:
                                madeOf = madeOf.Name(injectionInfo.MemberName);
                                break;
                            case MemberInjectionType.Method:
                                throw new NotSupportedException();
                        }
                    }
                }

                _container.Register(definition.ExportType, definition.ActivationType, reuse, madeOf);
            }
        }

        public void RegisterFactory<TResult>(Func<TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle) where TResult : class
        {
            IReuse reuse = null;

            switch (lifestyle)
            {
                case RegistrationLifestyle.Singleton:
                    reuse = new SingletonReuse();
                    break;
                case RegistrationLifestyle.SingletonPerScope:
                    reuse = new CurrentScopeReuse();
                    break;
                case RegistrationLifestyle.SingletonPerObjectGraph:
                    throw new NotSupportedException("");
            }

            _container.RegisterDelegate(r => factory(), reuse);
        }

        public void RegisterFactory<T1, TResult>(Func<T1, TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle) where TResult : class
        {
            IReuse reuse = null;

            switch (lifestyle)
            {
                case RegistrationLifestyle.Singleton:
                    reuse = new SingletonReuse();
                    break;
                case RegistrationLifestyle.SingletonPerScope:
                    reuse = new CurrentScopeReuse();
                    break;
                case RegistrationLifestyle.SingletonPerObjectGraph:
                    throw new NotSupportedException("");
            }

            _container.RegisterDelegate(r => factory(r.Resolve<T1>()), reuse);
        }

        public void RegisterFactory<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle) where TResult : class
        {
            IReuse reuse = null;

            switch (lifestyle)
            {
                case RegistrationLifestyle.Singleton:
                    reuse = new SingletonReuse();
                    break;
                case RegistrationLifestyle.SingletonPerScope:
                    reuse = new CurrentScopeReuse();
                    break;
                case RegistrationLifestyle.SingletonPerObjectGraph:
                    throw new NotSupportedException("");
            }

            _container.RegisterDelegate(r => factory(r.Resolve<T1>(), r.Resolve<T2>(), r.Resolve<T3>()), reuse);
        }
    }
}
