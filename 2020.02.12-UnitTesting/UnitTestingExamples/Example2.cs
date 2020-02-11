using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UnitTestingExamples
{
    [TestClass]
    public class Example2
    {
        #region SUT

        public class CsvParser
        {
            public CsvParser(Stream stream)
            {
                Stream = stream ?? throw new ArgumentNullException(nameof(stream));
            }

            public Stream Stream { get; }

            public IEnumerable<string[]> Parse()
            {
                using var reader = new StreamReader(Stream);
                while (!reader.EndOfStream)
                {
                    yield return ParseLine(reader.ReadLine());
                }
            }

            private static string[] ParseLine(string line)
            {
                return line.Split(',');
            }
        }

        #endregion SUT

        #region Tests

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void CanReadCsvData()
        {
            // Arrange
            string data = @"first,second,third
1,2,3";
            using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(data));

            var parser = new CsvParser(memoryStream);

            // Act
            List<string[]> parsedRows = parser.Parse().ToList();

            // Assert
            TestContext.WriteLine($"Found {parsedRows.Count} rows");
            Assert.AreEqual(2, parsedRows.Count);
            CollectionAssert.AreEqual(new[] { "first", "second", "third" }, parsedRows[0]);
            CollectionAssert.AreEqual(new[] { "1", "2", "3" }, parsedRows[1]);
        }

        #endregion Tests
    }
}
