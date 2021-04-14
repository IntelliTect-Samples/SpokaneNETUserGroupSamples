using System.CommandLine;
using System.CommandLine.IO;
using System.IO;

namespace FileConverter
{
    class Program
    {
        /// <summary>
        /// Demo app for converting files
        /// </summary>
        /// <param name="inputFile">The file to convert</param>
        /// <param name="outputDirectory">The output directory to covert files into.</param>
        /// <param name="outputType">The conversion type.</param>
        /// <param name="console">Generated console.</param>
        static void Main(
            FileInfo inputFile, 
            DirectoryInfo outputDirectory,
            OutputType outputType = OutputType.Yaml,
            IConsole console = null)
        {
            console.Out.WriteLine($"Processing {inputFile?.FullName} ({outputType}) => {outputDirectory?.FullName}");
        }
    }

    public enum OutputType
    {
        Unknown,
        Json,
        Csv,
        Yaml
    }
}
