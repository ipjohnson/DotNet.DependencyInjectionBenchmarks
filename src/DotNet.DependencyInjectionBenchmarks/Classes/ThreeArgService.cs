using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.DependencyInjectionBenchmarks.Classes
{
    public interface IThreeArgService1
    {
        int IntValue { get; }

        string StringValue { get; }

        ITransientService TransientService { get; }
    }

    public class ThreeArgService1 : IThreeArgService1
    {
        public ThreeArgService1(int intValue, string stringValue, ITransientService transientService)
        {
            IntValue = intValue;
            StringValue = stringValue;
            TransientService = transientService;
        }

        public int IntValue { get; }

        public string StringValue { get; }

        public ITransientService TransientService { get; }
    }
}
