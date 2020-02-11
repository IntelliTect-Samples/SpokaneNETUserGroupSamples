using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UnitTestingExamples
{
    [TestClass]
    public class Example3
    {
        #region SUT

        public class CsvParser
        {
            public CsvParser(IReadFile readFile)
            {
                File = readFile ?? throw new ArgumentNullException(nameof(readFile));
            }

            private IReadFile File { get; }

            public IEnumerable<string[]> Parse()
            {
                foreach (string line in File.GetLines())
                {
                    yield return ParseLine(line);
                }
            }

            private static string[] ParseLine(string line)
            {
                return line.Split(',');
            }
        }

        public interface IReadFile
        {
            IEnumerable<string> GetLines();
        }

        public class ReadFile : IReadFile
        {
            public ReadFile(string filePath)
            {
                FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            }

            public string FilePath { get; }

            public IEnumerable<string> GetLines() => File.ReadLines(FilePath);
        }

        #endregion SUT

        #region Tests

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void CanReadCsvData()
        {
            // Arrange
            var readFile = new TestableReadFile("first,second,third", "1,2,3");
            var parser = new CsvParser(readFile);

            // Act
            List<string[]> parsedRows = parser.Parse().ToList();

            // Assert
            TestContext.WriteLine($"Found {parsedRows.Count} rows");
            Assert.AreEqual(2, parsedRows.Count);
            CollectionAssert.AreEqual(new[] { "first", "second", "third" }, parsedRows[0]);
            CollectionAssert.AreEqual(new[] { "1", "2", "3" }, parsedRows[1]);
        }

        #endregion Tests

        #region Test Double

        public class TestableReadFile : IReadFile
        {
            public TestableReadFile(params string[] lines)
            {
                Lines = lines;
            }

            public string[] Lines { get; }

            public IEnumerable<string> GetLines() => Lines;
        }

        #endregion Test Double
    }
}
