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
    
    public interface IOneArgeFactoryService2
    {
        ISmallObjectGraphService2 SmallObject { get; }
    }

    public class OneArgeFactoryService2 : IOneArgeFactoryService2
    {
        public OneArgeFactoryService2(ISmallObjectGraphService2 smallObject)
        {
            SmallObject = smallObject;
        }

        public ISmallObjectGraphService2 SmallObject { get; }
    }
    
    public interface IOneArgeFactoryService3
    {
        ISmallObjectGraphService3 SmallObject { get; }
    }

    public class OneArgeFactoryService3 : IOneArgeFactoryService3
    {
        public OneArgeFactoryService3(ISmallObjectGraphService3 smallObject)
        {
            SmallObject = smallObject;
        }

        public ISmallObjectGraphService3 SmallObject { get; }
    }
}
