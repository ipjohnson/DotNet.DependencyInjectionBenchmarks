﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DryIoc;

namespace DotNet.DependencyInjectionBenchmarks.Containers
{
    public class DryIocContainer : IContainerScope
    {
        private readonly Container _container = new Container();

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _container.Dispose();
        }

        public class DryIocScope : IResolveScope
        {
            private readonly IContainer _scope;

            public DryIocScope(IContainer scope)
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
            var scope = _container.OpenScope(scopeName);

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

                _container.Register(definition.ExportType, definition.ActivationType, reuse);
            }
        }

        public void RegisterFactory<TResult>(Func<TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle)
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

        public void RegisterFactory<T1, TResult>(Func<T1, TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle)
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

        public void RegisterFactory<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> factory, RegistrationMode mode, RegistrationLifestyle lifestyle)
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