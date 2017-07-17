using System.Linq;
using System.Reflection;
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

            switcher.Run(args); //, new BenchmarkConfig());
        }
    }
}
