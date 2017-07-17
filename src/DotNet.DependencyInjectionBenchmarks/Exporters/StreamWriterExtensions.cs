using System.IO;

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
