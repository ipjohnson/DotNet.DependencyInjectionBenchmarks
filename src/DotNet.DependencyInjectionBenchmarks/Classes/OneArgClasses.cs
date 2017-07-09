using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.DependencyInjectionBenchmarks.Classes
{
    public interface IOneArgeFactoryService1
    {
        ISmallObjectGraphService1 SmallObject { get; }
    }

    public class OneArgeFactoryService1 : IOneArgeFactoryService1
    {
        public OneArgeFactoryService1(ISmallObjectGraphService1 smallObject)
        {
            SmallObject = smallObject;
        }

        public ISmallObjectGraphService1 SmallObject { get; }
    }
}
