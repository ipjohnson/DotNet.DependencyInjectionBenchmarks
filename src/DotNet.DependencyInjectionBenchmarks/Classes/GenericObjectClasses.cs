using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.DependencyInjectionBenchmarks.Classes
{
    public interface IGenericObjectService<T>
    {
        T Value { get; }
    }

    public class GenericObjectService<T>  : IGenericObjectService<T>
    {
        public GenericObjectService(T value)
        {
            Value = value;
        }

        public T Value { get; }
    }
}
