﻿namespace DotNet.DependencyInjectionBenchmarks.Classes
{
    public interface IOneArgeFactoryService
    {
        ISmallObjectService SmallObject { get; }
    }

    public class OneArgeFactoryService : IOneArgeFactoryService
    {
        public OneArgeFactoryService(ISmallObjectService smallObject)
        {
            SmallObject = smallObject;
        }

        public ISmallObjectService SmallObject { get; }
    }
}
