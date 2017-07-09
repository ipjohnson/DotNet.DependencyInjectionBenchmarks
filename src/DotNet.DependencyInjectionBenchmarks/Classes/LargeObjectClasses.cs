using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.DependencyInjectionBenchmarks.Classes
{
    public interface ILargeTransient1
    {
        ISmallObjectService SmallObject { get; }
    }

    public class LargeTransient1 : ILargeTransient1
    {
        public LargeTransient1(ISmallObjectService smallObject)
        {
            SmallObject = smallObject;
        }

        public ISmallObjectService SmallObject { get; }
    }

    public interface ILargeObjectService1
    {

    }

    public class LargeObjectService1
    {
    }
}
