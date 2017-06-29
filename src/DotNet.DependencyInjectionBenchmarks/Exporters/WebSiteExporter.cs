using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Parameters;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace DotNet.DependencyInjectionBenchmarks.Exporters
{
    public class WebSiteExporter : ExporterBase
    {
        public string OutputDirectory { get; set; } = "WebSite";

        public override void ExportToLog(Summary summary, ILogger logger)
        {
            var path = Path.Combine(summary.ResultsDirectoryPath, OutputDirectory);

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            Directory.CreateDirectory(path);

            var calculatedHeaders = GetHeaders(summary);

            ExportSummaryPage(path, summary, calculatedHeaders);
            
            foreach (var benchmarkInfo in calculatedHeaders.Benchmarks)
            {
                ExportBenchmarkPage(path, summary,calculatedHeaders, benchmarkInfo);
            }
        }

        private void ExportBenchmarkPage(string path, Summary summary, SummaryInfo calculatedHeaders, BenchmarkInfo info)
        {
            using (var file = File.Create(Path.Combine(path, info.BenchmarkName.Replace('|', '_') + ".html")))
            {
                using (var textFile = new StreamWriter(file))
                {
                    textFile.WriteLine("<html>");
                    textFile.Indent(1);
                    textFile.WriteLine("<head>");
                    textFile.Indent(2);
                    textFile.Write(_headString);
                    textFile.Indent(2);
                    textFile.WriteLine("</head>");
                    textFile.Indent(1);
                    textFile.WriteLine("<body>");

                    WriteMenu(summary, calculatedHeaders, textFile);

                    textFile.Indent(2);
                    textFile.WriteLine("<div class=\"container-fluid theme-showcase\" role=\"main\">");

                    WriteBenchmarkIntro(textFile, summary, info);

                    WriteBenchmarkTableData(textFile, summary, info);

                    OutputSummaryDetails(summary, textFile);

                    textFile.Write(_sciprtString);

                    textFile.Indent(1);
                    textFile.WriteLine("</div></body>");
                    textFile.WriteLine("</html>");
                }
            }
        }

        private void WriteBenchmarkIntro(StreamWriter textFile, Summary summary, BenchmarkInfo info)
        {
            var benchmark = summary.Benchmarks.First(b => b.Target.Type.Name == info.ClassName);

            var property = benchmark.Target.Type.GetTypeInfo().DeclaredProperties.FirstOrDefault(p => p.Name == "Description");

            var description = property?.GetValue(null) ?? "";

            textFile.WriteLine("<div class=\"jumbotron\">");
            textFile.WriteLine($"<h1>{info.ClassName.Replace("Benchmark", "")}</h1>");
            textFile.WriteLine($"<p>{description}</p>");
            textFile.WriteLine("</div>");
        }

        private void WriteBenchmarkTableData(StreamWriter textFile, Summary summary, BenchmarkInfo info)
        {
            var reports = summary.Reports.Where(r => r.Benchmark.Target.Type.Name == info.ClassName);

            if (info.Parameters.Count > 0)
            {
                reports = reports.Where(r =>
                {
                    foreach (var parameter in info.Parameters)
                    {
                        if (!r.Benchmark.Parameters.Items.Any(
                            p => p.Name == parameter.Name && p.Value == parameter.Value))
                        {
                            return false;
                        }
                    }

                    return true;
                });
            }

            textFile.Indent(4);
            textFile.WriteLine("<table class=\"table table-striped table-bordered responsive nowrap\" id=\"benchmarkDataTable\" width=\"100%\" cellspacing=\"0\">");

            textFile.WriteLine("<thead>");
            textFile.WriteLine("<tr>");
            textFile.WriteLine("<th>Container</th>");
            textFile.WriteLine("<th>Env</th>");
            textFile.WriteLine("<th>Mean</th>");
            textFile.WriteLine("<th>Median</th>");
            textFile.WriteLine("<th>Max</th>");
            textFile.WriteLine("<th>Std Dev</th>");
            textFile.WriteLine("<th>Std Err</th>");
            textFile.WriteLine("<th>Gen 1</th>");
            textFile.WriteLine("<th>Gen 2</th>");
            textFile.WriteLine("<th>Bytes Alloc</th>");
            textFile.WriteLine("</tr>");
            textFile.WriteLine("</thead>");

            textFile.WriteLine("<tbody>");
            foreach (var report in reports)
            {
                if (report.ResultStatistics != null)
                {
                    textFile.Write("<tr>");
                    textFile.Write($"<td>{report.Benchmark.Target.Method.Name}</td>");
                    textFile.Write($"<td>{report.Benchmark.Job.ResolvedId}</td>");
                    textFile.Write($"<td style=\"text-align: right\">{report.ResultStatistics.Mean:F1}</td>");
                    textFile.Write($"<td style=\"text-align: right\">{report.ResultStatistics.Median:F1}</td>");
                    textFile.Write($"<td style=\"text-align: right\">{report.ResultStatistics.Max:F1}</td>");
                    textFile.Write(
                        $"<td style= \"text-align: right\">{report.ResultStatistics.StandardDeviation:F3}</td>");
                    textFile.Write($"<td style=\"text-align: right\">{report.ResultStatistics.StandardError:F3}</td>");
                    textFile.Write($"<td style=\"text-align: right\">{report.GcStats.Gen1Collections}</td>");
                    textFile.Write($"<td style=\"text-align: right\">{report.GcStats.Gen2Collections}</td>");
                    textFile.Write($"<td style=\"text-align: right\">{report.GcStats.BytesAllocatedPerOperation}</td>");
                    textFile.WriteLine("</tr>");
                }
            }

            textFile.WriteLine("</tbody>");

            textFile.Indent(4);
            textFile.WriteLine("</table>");
        }

        private void ExportSummaryPage(string path, Summary summary, SummaryInfo calculatedHeaders)
        {
         using (var file = File.Create(Path.Combine(path, "Index.html")))
            {
                using (var textFile = new StreamWriter(file))
                {
                    textFile.WriteLine("<html>");
                    textFile.Indent(1);
                    textFile.WriteLine("<head>");
                    textFile.Indent(2);
                    textFile.Write(_headString);
                    textFile.Indent(2);
                    textFile.WriteLine("</head>");
                    textFile.Indent(1);
                    textFile.WriteLine("<body>");

                    WriteMenu(summary,calculatedHeaders, textFile);

                    textFile.Indent(2);
                    textFile.WriteLine("<div class=\"container-fluid theme-showcase\" role=\"main\">");

                    textFile.Write(_introHtml);

                    OutputDataTable(summary, textFile);

                    OutputSummaryDetails(summary, textFile);

                    textFile.Write(_sciprtString);

                    textFile.Indent(1);
                    textFile.WriteLine("</div></body>");
                    textFile.WriteLine("</html>");
                }
            }
        }

        private const string DetailsOpen = @"
<div class=""row"">
        <div class=""col-sm-12"">
            <a href = ""#details"" class=""btn btn-info"" data-toggle=""collapse"">Details</a>
        <div id = ""details"" class=""collapse"">
            <pre><code>";

        private const string DetailsClose = @"</code></pre></div></div></div>";

        private void OutputSummaryDetails(Summary summary, StreamWriter textFile)
        {
            textFile.WriteLine(DetailsOpen);

            foreach (var hostInfo in summary.HostEnvironmentInfo.ToFormattedString())
            {
                textFile.WriteLine(hostInfo);
            }

            textFile.WriteLine(summary.AllRuntimes);

            textFile.WriteLine(DetailsClose);
        }

        private void WriteMenu(Summary summary, SummaryInfo calculatedHeaders, StreamWriter textFile)
        {
            //var benchmarks = new Dictionary<string, List<Tuple<string, Benchmark>>>();

            //foreach (var report in summary.Reports)
            //{           

            //    foreach (var category in report.Benchmark.Target.Categories)
            //    {
            //        if (!benchmarks.TryGetValue(category, out List<Tuple<string, Benchmark>> benchmarkList))
            //        {
            //            benchmarkList = new List<Tuple<string, Benchmark>>();
            //            benchmarks[category] = benchmarkList;
            //        }

            //        if (benchmarkList.Count == 0 ||
            //            benchmarkList.All(b => b.Item2.Target.Type != report.Benchmark.Target.Type))
            //        {
            //            var displayString = report.Benchmark.Target.Type.Name;

            //            displayString = displayString.Replace("Benchmark", "");

            //            displayString = FriendlyName(displayString);

            //            benchmarkList.Add(new Tuple<string, Benchmark>(displayString, report.Benchmark));
            //        }
            //    }
            //}

            var categoryDictionary = new Dictionary<string, List<Tuple<string, BenchmarkInfo>>>();

            foreach (var calculatedHeadersBenchmark in calculatedHeaders.Benchmarks)
            {
                foreach (var category in calculatedHeadersBenchmark.Categories)
                {
                    if (!categoryDictionary.TryGetValue(category, out List<Tuple<string, BenchmarkInfo>> list))
                    {
                        list = new List<Tuple<string, BenchmarkInfo>>();

                        categoryDictionary[category] = list;
                    }

                    list.Add(new Tuple<string, BenchmarkInfo>(calculatedHeadersBenchmark.DisplayName, calculatedHeadersBenchmark));
                }
            }

            var sortList = new List<KeyValuePair<string,List<Tuple<string,BenchmarkInfo>>>>(categoryDictionary);

            sortList.Sort((x, y) => string.Compare(x.Key, y.Key));

            var standard = sortList.Find(k => k.Key == "Standard");

            if (standard.Key != null)
            {
                sortList.Remove(standard);
                sortList.Insert(0, standard);
            }

            var menuString = new StringBuilder();

            foreach (var pair in sortList)
            {
                pair.Value.Sort((x, y) => string.CompareOrdinal(x.Item1, y.Item1));

                menuString.AppendLine("<li class=\"dropdown\">");
                menuString.AppendLine($"<a href=\"#\" class=\"dropdown-toggle\" data-toggle=\"dropdown\" role=\"button\" aria-haspopup=\"true\" aria-expanded=\"false\">{pair.Key} <span class=\"caret\"></span></a>");
                menuString.AppendLine("<ul class=\"dropdown-menu\">");

                foreach (var tuple in pair.Value)
                {
                    menuString.AppendLine($"<li><a href=\"{tuple.Item2.BenchmarkName.Replace('|','_')}.html\">{tuple.Item1}</a></li>");
                }

                menuString.AppendLine("</ul>");
                menuString.AppendLine("</li>");
            }

            var menu = _menuHtml.Replace("{menu_tags}", menuString.ToString());

            textFile.Write(menu);
        }

        private const string _menuHtml = @"
    <nav class=""navbar navbar-inverse navbar-fixed-top"">
        <div class=""container"">
            <div class=""navbar-header"">
                <a class=""navbar-brand"" href=""Index.html"">Benchmarks</a>
            </div>
            <div id=""navbar"" class=""navbar-collapse collapse"">
                <ul class=""nav navbar-nav"">
{menu_tags}
                </ul>
            </div>
        </div>
    </nav>
";

        private void OutputDataTable(Summary summary, StreamWriter textFile)
        {
            textFile.Indent(4);
            textFile.WriteLine("<table class=\"table table-striped table-bordered responsive nowrap\" id=\"benchmarkDataTable\" width=\"100%\" cellspacing=\"0\">");

            var calculatedHeaders = GetHeaders(summary);

            WriteTableHeader(textFile, calculatedHeaders);

            WriteTableData(textFile, summary, calculatedHeaders);

            textFile.Indent(4);
            textFile.WriteLine("</table>");
        }

        private void WriteTableData(StreamWriter textFile, Summary summary, SummaryInfo calculatedHeaders)
        {
            textFile.Indent(4);
            textFile.WriteLine("<tbody>");
            foreach (var method in calculatedHeaders.Methods)
            {
                var index = method.IndexOf('|');
                var name = method.Substring(0, index);
                var env = method.Substring(index + 1);

                textFile.Indent(5);
                textFile.Write("<tr>");
                textFile.Write($"<td>{name}</td>");
                textFile.Write($"<td>{env}</td>");

                foreach (var benchmark in calculatedHeaders.Benchmarks)
                {
                    var report =
                        summary.Reports.FirstOrDefault(r => GetBenchmarkName(r.Benchmark) == benchmark.BenchmarkName &&
                                                        r.Benchmark.Job.ResolvedId == env &&
                                                        r.Benchmark.Target.Method.Name == name);

                    if (report?.ResultStatistics != null)
                    {
                        textFile.Write($"<td style=\"text-align: right\">{report.ResultStatistics.Mean:F1}</td>");
                    }
                    else
                    {
                        textFile.Write("<td>&nbsp;</td>");
                    }
                }

                textFile.WriteLine("</tr>");
            }

            textFile.Indent(4);
            textFile.WriteLine("</tbody>");
        }

        private void WriteTableHeader(StreamWriter textFile, SummaryInfo calculatedHeaders)
        {
            textFile.Indent(4);
            textFile.WriteLine("<thead>");
            textFile.Indent(5);
            textFile.WriteLine("<tr>");

            textFile.Indent(6);
            textFile.WriteLine("<th>Container</th>");

            textFile.Indent(6);
            textFile.WriteLine("<th>Env</th>");

            foreach (var benchmark in calculatedHeaders.Benchmarks)
            {
                textFile.Indent(6);
                textFile.WriteLine($"<th>{benchmark.DisplayName}</th>");
            }

            textFile.Indent(5);
            textFile.WriteLine("</tr>");
            textFile.Indent(4);
            textFile.WriteLine("</thead>");
        }

        private class BenchmarkInfo
        {
            public string ClassName { get; set; }

            public string BenchmarkName { get; set; }

            public IReadOnlyList<ParameterInstance> Parameters { get; set; } =
                Grace.Data.Immutable.ImmutableArray<ParameterInstance>.Empty;

            public string DisplayName { get; set; }

            public string[] Categories { get; set; }
        }

        private class SummaryInfo
        {
            public IEnumerable<BenchmarkInfo> Benchmarks { get; set; }

            public IEnumerable<string> Methods { get; set; }
        }

        private SummaryInfo GetHeaders(Summary summary)
        {
            var benchmarks = new Dictionary<string, BenchmarkInfo>();
            var methods = new List<string>();

            foreach (var report in summary.Reports)
            {
                var benchmarkName = GetBenchmarkName(report.Benchmark);

                if (!benchmarks.ContainsKey(benchmarkName))
                {
                    var categories = report.Benchmark.Target.Categories.ToList();

                    foreach (var methodInfo in report.Benchmark.Target.Type.GetMethods())
                    {
                        categories.Remove(methodInfo.Name);
                    }

                    benchmarks.Add(benchmarkName, new BenchmarkInfo
                    {
                        BenchmarkName = benchmarkName,
                        DisplayName = GetBenchmarkDisplayName(report.Benchmark),
                        Categories = categories.ToArray(),
                        ClassName = report.Benchmark.Target.Type.Name,
                        Parameters = report.Benchmark.Parameters.Items
                    });
                }

                var name = GetContainerNameAndClr(report);

                if (!methods.Contains(name))
                {
                    methods.Add(name);
                }
            }

            return new SummaryInfo { Benchmarks = benchmarks.Values, Methods = methods };
        }

        private string GetBenchmarkName(Benchmark benchmark)
        {
            var parameters = "";

            if (benchmark.Parameters != null && benchmark.Parameters.Count > 0)
            {
                foreach (var parameter in benchmark.Parameters.Items)
                {
                    parameters += "|" + parameter.Value;
                }
            }

            return benchmark.Target.Type.Name + parameters;
        }

        private string GetBenchmarkDisplayName(Benchmark benchmark)
        {
            var parameters = "";

            if (benchmark.Parameters != null && benchmark.Parameters.Count > 0)
            {
                foreach (var parameter in benchmark.Parameters.Items)
                {
                    parameters += " " + parameter.Value;
                }
            }

            return benchmark.Target.Type.Name.Replace("Benchmark", "") + parameters;

        }

        private string GetContainerNameAndClr(BenchmarkReport report)
        {
            return report.Benchmark.Target.Method.Name + "|" + report.Benchmark.Job.ResolvedId;
        }

        private const string _headString = @"
        <link rel=""stylesheet"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"" integrity=""sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u"" crossorigin=""anonymous"">
        <link rel=""stylesheet"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css"" integrity=""sha384-rHyoN1iRsVXV4nD0JutlnGaslCJuC7uwjduW9SVrLvRYooPp2bWYgmgJQIXwl/Sp"" crossorigin=""anonymous"">
        <link rel=""stylesheet"" href=""https://cdn.datatables.net/v/bs/dt-1.10.15/b-1.3.1/b-colvis-1.3.1/cr-1.3.3/fh-3.1.2/r-2.1.1/datatables.min.css"">
";

        private const string _introHtml = @"
        <div class=""jumbotron"">
            <h1>Dependency Injection Benchmarks</h1>
            <p>Comprehensive benchmarks for .net dependency injection containers.</p>
        </div>
";

        private const string _sciprtString = @"
		<script src=""https://code.jquery.com/jquery-1.12.4.js""></script>
        <script src=""https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js""></script>
        <script src=""https://cdn.datatables.net/v/bs/dt-1.10.15/b-1.3.1/b-colvis-1.3.1/cr-1.3.3/fh-3.1.2/r-2.1.1/datatables.min.js""></script>
        <script>
            $(document).ready(function() {
                $('#benchmarkDataTable').DataTable();
            } );
        </script>
";

        private string FriendlyName(string name)
        {
            var displayString = name;

            for (var i = 1; i < displayString.Length; i++)
            {
                if (char.IsUpper(displayString[i]))
                {
                    displayString = displayString.Insert(i, " ");
                    i++;
                }
            }

            return displayString;
        }

    }
}
