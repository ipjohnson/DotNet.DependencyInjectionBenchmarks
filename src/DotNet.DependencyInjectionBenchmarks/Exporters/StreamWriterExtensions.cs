using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotNet.DependencyInjectionBenchmarks.Exporters
{
    public static class StreamWriterExtensions
    {
        public static void Indent(this StreamWriter writer, int indentCount)
        {
            writer.Write(new string('\t', indentCount));
        }
    }
}
