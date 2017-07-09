using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.DependencyInjectionBenchmarks.Classes
{
    public interface IOneArgeFactoryService1
    {
        ISmallObjectService SmallObject { get; }
    }

    public class OneArgeFactoryService1 : IOneArgeFactoryService1
    {
        public OneArgeFactoryService1(ISmallObjectService smallObject)
        {
            SmallObject = smallObject;
        }

        public ISmallObjectService SmallObject { get; }
    }
}
