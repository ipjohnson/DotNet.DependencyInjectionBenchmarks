using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.DependencyInjectionBenchmarks.Classes
{
    public interface IEnumerableService
    {
        ISingletonService1 Singleton { get; }

        ITransientService1 Transient { get; }
    }

    public class EnumerableService1 : IEnumerableService
    {
        public EnumerableService1(ITransientService1 transient, ISingletonService1 singleton)
        {
            Transient = transient;
            Singleton = singleton;
        }

        public ISingletonService1 Singleton { get; }

        public ITransientService1 Transient { get; }
    }

    public class EnumerableService2 : IEnumerableService
    {
        public EnumerableService2(ITransientService1 transient, ISingletonService1 singleton)
        {
            Transient = transient;
            Singleton = singleton;
        }

        public ISingletonService1 Singleton { get; }

        public ITransientService1 Transient { get; }
    }

    public class EnumerableService3 : IEnumerableService
    {
        public EnumerableService3(ITransientService1 transient, ISingletonService1 singleton)
        {
            Transient = transient;
            Singleton = singleton;
        }

        public ISingletonService1 Singleton { get; }

        public ITransientService1 Transient { get; }
    }

    public class EnumerableService4 : IEnumerableService
    {
        public EnumerableService4(ITransientService1 transient, ISingletonService1 singleton)
        {
            Transient = transient;
            Singleton = singleton;
        }

        public ISingletonService1 Singleton { get; }

        public ITransientService1 Transient { get; }
    }

    public class EnumerableService5 : IEnumerableService
    {
        public EnumerableService5(ITransientService1 transient, ISingletonService1 singleton)
        {
            Transient = transient;
            Singleton = singleton;
        }

        public ISingletonService1 Singleton { get; }

        public ITransientService1 Transient { get; }
    }
}
