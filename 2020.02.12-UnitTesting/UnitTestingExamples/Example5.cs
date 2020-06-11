using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnitTestingExamples
{
    [TestClass]
    public class Example5
    {
        #region SUT

        public class CsvParser
        {
            private ILineSource LineSource { get; }
            public ILogger Logger { get; }

            public CsvParser(ILineSource lineSource, ILoggerFactory loggerFactory)
            {
                LineSource = lineSource ?? throw new ArgumentNullException(nameof(lineSource));
                if (loggerFactory is null) throw new ArgumentNullException(nameof(loggerFactory));
                Logger = loggerFactory.CreateLogger<CsvParser>();
            }

            public IEnumerable<string[]> Parse()
            {
                foreach (string line in LineSource.GetLines())
                {
                    yield return ParseLine(line);
                }
            }

            private static string[] ParseLine(string line)
            {
                return line.Split(',');
            }
        }

        public interface ILineSource
        {
            IEnumerable<string> GetLines();
        }

        public class FileLineSource : ILineSource
        {
            public FileLineSource(string filePath)
            {
                FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            }

            private string FilePath { get; }

            public IEnumerable<string> GetLines()
            {
                try
                {
                    return File.ReadLines(FilePath);
                }
                catch (IOException)
                {
                    return Enumerable.Empty<string>();
                }
            }
        }

        #endregion SUT

        #region Tests

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void CanReadCsvData()
        {
            // Arrange
            var mocker = new AutoMocker();
            mocker
                .Setup<ILineSource, IEnumerable<string>>(x => x.GetLines())
                .Returns(new[] { "first,second,third", "1,2,3" });

            CsvParser parser = mocker.CreateInstance<CsvParser>();

            // Act
            List<string[]> parsedRows = parser.Parse().ToList();

            // Assert
            TestContext.WriteLine($"Found {parsedRows.Count} rows");
            Assert.AreEqual(2, parsedRows.Count);
            CollectionAssert.AreEqual(new[] { "first", "second", "third" }, parsedRows[0]);
            CollectionAssert.AreEqual(new[] { "1", "2", "3" }, parsedRows[1]);

            mocker.Verify<ILineSource>(x => x.GetLines(), Times.Once());
        }

        #endregion Tests
    }
}
