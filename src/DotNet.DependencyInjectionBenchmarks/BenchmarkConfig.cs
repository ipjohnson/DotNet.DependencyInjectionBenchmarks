using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Jobs;
using DotNet.DependencyInjectionBenchmarks.Exporters;

namespace DotNet.DependencyInjectionBenchmarks
{
    public class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            Add(Job.ShortRun);
            Add(new MemoryDiagnoser());
            Add(new CompositeExporter(CsvExporter.Default, HtmlExporter.Default, MarkdownExporter.Default, new WebSiteExporter()));
        }
    }
}
