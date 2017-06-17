using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.DependencyInjectionBenchmarks.Classes
{
    public interface IThreeArgService1
    {
        int IntValue { get; }

        string StringValue { get; }

        ITransientService1 TransientService { get; }
    }

    public class ThreeArgService1 : IThreeArgService1
    {
        public ThreeArgService1(int intValue, string stringValue, ITransientService1 transientService)
        {
            IntValue = intValue;
            StringValue = stringValue;
            TransientService = transientService;
        }

        public int IntValue { get; }

        public string StringValue { get; }

        public ITransientService1 TransientService { get; }
    }

    public interface IThreeArgService2
    {
        int IntValue { get; }

        string StringValue { get; }

        ITransientService2 TransientService { get; }
    }

    public class ThreeArgService2 : IThreeArgService2
    {
        public ThreeArgService2(int intValue, string stringValue, ITransientService2 transientService)
        {
            IntValue = intValue;
            StringValue = stringValue;
            TransientService = transientService;
        }

        public int IntValue { get; }

        public string StringValue { get; }

        public ITransientService2 TransientService { get; }
    }

    public interface IThreeArgService3
    {
        int IntValue { get; }

        string StringValue { get; }

        ITransientService3 TransientService { get; }
    }

    public class ThreeArgService3 : IThreeArgService3
    {
        public ThreeArgService3(int intValue, string stringValue, ITransientService3 transientService)
        {
            IntValue = intValue;
            StringValue = stringValue;
            TransientService = transientService;
        }

        public int IntValue { get; }

        public string StringValue { get; }

        public ITransientService3 TransientService { get; }
    }
}
