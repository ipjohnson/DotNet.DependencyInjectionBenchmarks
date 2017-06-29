using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.DependencyInjectionBenchmarks.Classes
{
    public interface ILargeTransient1
    {
        ISingletonService1 SingletonService1 { get; }
    }

    public class LargeTransient1 : ILargeTransient1
    {
        public ISingletonService1 SingletonService1 { get; }
    }

    public interface ILargeObjectService1
    {
        
    }

    public class LargeObjectService1
    {
    }
}
