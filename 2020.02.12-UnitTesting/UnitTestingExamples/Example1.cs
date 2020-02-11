using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnitTestingExamples
{
    [TestClass]
    public class Example1
    {
        #region SUT

        public class CsvParser
        {
            public CsvParser(FileInfo file)
            {
                File = file ?? throw new ArgumentNullException(nameof(file));
            }

            public FileInfo File { get; }

            public IEnumerable<string[]> Parse()
            {
                using var reader = new StreamReader(File.OpenRead());
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

        private string TestFile { get; set; }

        [TestInitialize]
        public void TestSetup()
        {
            TestFile = Path.GetTempFileName();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            try
            {
                File.Delete(TestFile);
            }
            catch
            {
                //Ignoring error because this is a fire and forget
            }
        }

        [TestMethod]
        public void CanReadCsvData()
        {
            // Arrange
            string data = @"first,second,third
1,2,3";
            File.WriteAllText(TestFile, data);

            var parser = new CsvParser(new FileInfo(TestFile));

            // Act
            List<string[]> parsedRows = parser.Parse().ToList();

            // Assert
            TestContext.WriteLine($"Found {parsedRows.Count} rows from {TestFile}");
            Assert.AreEqual(2, parsedRows.Count);
            CollectionAssert.AreEqual(new[] { "first", "second", "third" }, parsedRows[0]);
            CollectionAssert.AreEqual(new[] { "1", "2", "3" }, parsedRows[1]);
        }

        #endregion Tests
    }
}
