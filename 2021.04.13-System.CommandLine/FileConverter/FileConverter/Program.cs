using System;
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
        /// <param name="ouputType">The conversion type.</param>
        static void Main(
            FileInfo inputFile, 
            DirectoryInfo outputDirectory,
            OutputType ouputType = OutputType.Yaml)
        {
            Console.WriteLine("Hello World!");
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
