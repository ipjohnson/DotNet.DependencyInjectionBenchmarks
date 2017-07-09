using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.DependencyInjectionBenchmarks.Classes
{
    public interface ILargeTransient1
    {
        ISmallObjectGraphService1 SmallObject { get; }
    }

    public class LargeTransient1 : ILargeTransient1
    {
        public ISmallObjectGraphService1 SmallObject { get; }
    }

    public interface ILargeObjectService1
    {

    }

    public class LargeObjectService1
    {
    }

    public interface ILargeTransient2
    {
        ISingletonService2 SingletonService2 { get; }
    }

    public class LargeTransient2 : ILargeTransient2
    {
        public ISingletonService2 SingletonService2 { get; }
    }

    public interface ILargeObjectService2
    {

    }

    public class LargeObjectService2
    {
    }

}
