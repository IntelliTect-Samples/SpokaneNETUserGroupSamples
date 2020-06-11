using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace UnitTestingExamples
{
    [TestClass]
    public class Example6
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

        #region Simulator

        public class LineSourceSimulator : ILineSource
        {
            public LineSourceSimulator(string[] lines)
            {
                Lines = lines ?? Array.Empty<string>();
            }

            public string[] Lines { get; }

            public IEnumerable<string> GetLines() => Lines;
        }

        #endregion Simulator

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void CanReadCsvData()
        {
            // Arrange
            var mocker = new AutoMocker();
            string[] csvLines =
            {
                "first,second,third",
                "1,2,3"
            };
            mocker.Use<ILineSource>(new LineSourceSimulator(csvLines));

            CsvParser parser = mocker.CreateInstance<CsvParser>();

            // Act
            List<string[]> parsedRows = parser.Parse().ToList();

            // Assert
            TestContext.WriteLine($"Found {parsedRows.Count} rows");
            Assert.AreEqual(2, parsedRows.Count);
            CollectionAssert.AreEqual(new[] { "first", "second", "third" }, parsedRows[0]);
            CollectionAssert.AreEqual(new[] { "1", "2", "3" }, parsedRows[1]);
        }

        #region Simulator Tests v1

        public abstract class LineSourceTests
        {
            protected abstract ILineSource GetLineSourceWithData(string[] data);

            protected abstract ILineSource GetLineSourceWithoutData();

            [TestMethod]
            public void CanRetrieveValidData()
            {
                // Arrange
                string[] csvLines =
                {
                    "first,second,third",
                    "1,2,3"
                };

                ILineSource lineSource = GetLineSourceWithData(csvLines);

                // Act
                string[] lines = lineSource.GetLines().ToArray();

                // Assert
                CollectionAssert.AreEqual(csvLines, lines);
            }

            [TestMethod]
            public void ReturnsEmptyOnFailure()
            {
                // Arrange
                ILineSource lineSource = GetLineSourceWithoutData();

                // Act
                IEnumerable<string> lines = lineSource.GetLines();

                // Assert
                Assert.IsFalse(lines.Any());
            }
        }

        [TestClass]
        public class LineSourceSimulatorTests : LineSourceTests
        {
            protected override ILineSource GetLineSourceWithData(string[] data)
                => new LineSourceSimulator(data);

            protected override ILineSource GetLineSourceWithoutData()
                => new LineSourceSimulator(null);
        }

        [TestClass]
        public class FileLineSourceTests : LineSourceTests
        {
            private string TestFile { get; set; }

            [TestCleanup]
            public void TestCleanup()
            {
                if (TestFile is { } testFile)
                {
                    File.Delete(testFile);
                }
            }

            protected override ILineSource GetLineSourceWithData(string[] data)
            {
                string testFile = TestFile = Path.GetTempFileName();
                File.WriteAllLines(testFile, data);
                return new FileLineSource(testFile);
            }

            protected override ILineSource GetLineSourceWithoutData()
                => new FileLineSource("FileDoesNotExist" + Guid.NewGuid());
        }

        #endregion Simulator Tests v1

        #region Simulator Tests v2

        [TestMethod]
        [DynamicData(nameof(LineSourcesWithoutData), DynamicDataSourceType.Method)]
        public void ReturnsEmptyOnFailure(ILineSource lineSource)
        {
            // Arrange

            // Act
            IEnumerable<string> lines = lineSource.GetLines();

            // Assert
            Assert.IsFalse(lines.Any());
        }

        public static IEnumerable<object[]> LineSourcesWithoutData()
        {
            yield return new object[] { new LineSourceSimulator(null) };
            yield return new object[] { new FileLineSource("FileDoesNotExist" + Guid.NewGuid()) };
        }

        #endregion Simulator Tests v2

        #region Simulator Tests v3

        [TestMethod]
        [LineSourceData]
        public void ReturnsEmptyOnFailureFromAttributeSource(ILineSource lineSource)
        {
            // Arrange

            // Act
            IEnumerable<string> lines = lineSource.GetLines();

            // Assert
            Assert.IsFalse(lines.Any());
        }

        public class LineSourceDataAttribute : Attribute, ITestDataSource
        {
            public IEnumerable<object[]> GetData(MethodInfo methodInfo)
            {
                yield return new object[] { new LineSourceSimulator(null) };
                yield return new object[] { new FileLineSource("FileDoesNotExist" + Guid.NewGuid()) };
            }

            public string GetDisplayName(MethodInfo methodInfo, object[] data)
            {
                return $"{methodInfo.Name} with {data[0].GetType().Name}";
            }
        }

        #endregion Simulator Tests v3

        #endregion Tests
    }
}
