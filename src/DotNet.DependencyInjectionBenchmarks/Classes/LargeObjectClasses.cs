namespace DotNet.DependencyInjectionBenchmarks.Classes
{
    #region interfaces
    public interface ILargeSingletonService1
    {
        int IntValue { get; }
    }
    
    public interface ILargeSingletonService2
    {
        int IntValue { get; }
    }

    public interface ILargeSingletonService3
    {
        int IntValue { get; }
    }

    public interface ILargeTransient1
    {
        int IntValue { get; }
    }

    public interface ILargeTransient2
    {
        int IntValue { get; }
    }

    public interface ILargeTransient3
    {
        int IntValue { get; }
    }

    public interface ILargeObjectService1
    {
        ILargeTransient1 Transient { get; }

        ILargeSingletonService1 Singleton { get; }
    }
    
    public interface ILargeObjectService2
    {
        ILargeTransient2 Transient { get; }

        ILargeSingletonService2 Singleton { get; }
    }
    
    public interface ILargeObjectService3
    {
        ILargeTransient3 Transient { get; }

        ILargeSingletonService3 Singleton { get; }
    }

    public interface ILargeComplexService
    {
        ILargeObjectService1 Service1 { get; }

        ILargeObjectService1 Service2 { get; }

        ILargeObjectService1 Service3 { get; }
    }
    #endregion

    #region Classes

    public class LargeSingletonService1 : ILargeSingletonService1
    {
        public int IntValue { get; set; }
    }

    public class LargeSingletonService2 : ILargeSingletonService2
    {
        public int IntValue { get; set; }
    }

    public class LargeSingletonService3 : ILargeSingletonService3
    {
        public int IntValue { get; set; }
    }

    public class LargeTransient1 : ILargeTransient1
    {
        public int IntValue { get; set; }
    }

    public class LargeTransient2 : ILargeTransient2
    {
        public int IntValue { get; set; }
    }

    public class LargeTransient3 : ILargeTransient3
    {
        public int IntValue { get; set; }
    }

    public class LargeObjectService1 : ILargeObjectService1
    {
        public LargeObjectService1(ILargeTransient1 transient, ILargeSingletonService1 singleton)
        {
            Transient = transient;
            Singleton = singleton;
        }

        public ILargeTransient1 Transient { get; }

        public ILargeSingletonService1 Singleton { get; }
    }

    public class LargeObjectService2 : ILargeObjectService2
    {
        public LargeObjectService2(ILargeTransient2 transient, ILargeSingletonService2 singleton)
        {
            Transient = transient;
            Singleton = singleton;
        }

        public ILargeTransient2 Transient { get; }

        public ILargeSingletonService2 Singleton { get; }
    }

    public class LargeObjectService3 : ILargeObjectService3
    {
        public LargeObjectService3(ILargeTransient3 transient, ILargeSingletonService3 singleton)
        {
            Transient = transient;
            Singleton = singleton;
        }

        public ILargeTransient3 Transient { get; }

        public ILargeSingletonService3 Singleton { get; }
    }

    public class LargeComplexService : ILargeComplexService
    {
        public LargeComplexService(ILargeObjectService1 service1, ILargeObjectService1 service2, ILargeObjectService1 service3)
        {
            Service1 = service1;
            Service2 = service2;
            Service3 = service3;
        }

        public ILargeObjectService1 Service1 { get; }

        public ILargeObjectService1 Service2 { get; }

        public ILargeObjectService1 Service3 { get; }
    }
    #endregion
}
