using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;

namespace DotNet.DependencyInjectionBenchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var benchmarks =
                typeof(Program).GetTypeInfo().Assembly.ExportedTypes.Where(t => !t.GetTypeInfo().IsAbstract && t.Name.EndsWith("Benchmark"));

            var switcher = new BenchmarkSwitcher(benchmarks.ToArray());

            switcher.Run(args, new BenchmarkConfig());
        }
    }
}
