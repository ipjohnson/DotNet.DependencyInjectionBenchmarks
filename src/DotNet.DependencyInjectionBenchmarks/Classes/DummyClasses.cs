using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace DotNet.DependencyInjectionBenchmarks.Classes
{
    public static class DummyClasses
    {
        private static readonly object _lock = new object();
        private static readonly List<Type> _types = new List<Type>();
        private static readonly ModuleBuilder _moduleBuilder;

        static DummyClasses()
        {
            var dynamicAssembly = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(Guid.NewGuid().ToString()), AssemblyBuilderAccess.Run);

            _moduleBuilder = dynamicAssembly.DefineDynamicModule("DummyTypes");
        }

        public static IEnumerable<Type> GetTypes(int count)
        {
            if (count <= (_types.Count - 1))
            {
                return _types.GetRange(0, count);
            }

            GenerateTypes(_types.Count, count - _types.Count);

            return _types;
        }

        private static void GenerateTypes(int index, int count)
        {
            lock (_lock)
            {
                for (var i = index; i < index + count; i++)
                {
                    var proxyBuilder = _moduleBuilder.DefineType("DummyType" + i,
                        TypeAttributes.Class | TypeAttributes.Public);

                    _types.Add(proxyBuilder.CreateTypeInfo().AsType());
                }
            }
        }
    }
}
