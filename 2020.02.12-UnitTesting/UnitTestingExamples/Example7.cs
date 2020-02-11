using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace UnitTestingExamples
{
    [TestClass]
    public class Example7
    {
        #region SUT

        public interface ILineSource
        {
            event EventHandler<string> ErrorOccured;
            IEnumerable<string> GetLines();
        }

        public interface IAsyncLogger
        {
            Task LogAsync(string message);
        }

        public class CsvParser
        {
            private ILineSource LineSource { get; }
            public IAsyncLogger Logger { get; }

            public CsvParser(ILineSource lineSource, IAsyncLogger logger)
            {
                LineSource = lineSource ?? throw new ArgumentNullException(nameof(lineSource));
                Logger = logger ?? throw new ArgumentNullException(nameof(logger));
                LineSource.ErrorOccured += LineSource_ErrorOccured;
            }

            private async void LineSource_ErrorOccured(object sender, string message)
            {
                await Logger.LogAsync(message);
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

        #endregion SUT

        #region Tests

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void WhenErrorOccursItIsLogged()
        {
            // Arrange
            var mocker = new AutoMocker();
            var lineSource = new LineSourceSimulator(null);
            lineSource.WithError("Crash");
            mocker.Use<ILineSource>(lineSource);

            //Force the async call to complete synchronously 
            mocker.Setup<IAsyncLogger, Task>(x => x.LogAsync("Crash"))
                .Returns(Task.CompletedTask);

            var parser = mocker.CreateInstance<CsvParser>();

            // Act
            var rows = parser.Parse().ToArray();

            // Assert
            Assert.AreEqual(0, rows.Length);
            mocker.VerifyAll();
        }

        #endregion Tests

        #region Simulator

        public class LineSourceSimulator : ILineSource
        {
            public LineSourceSimulator(string[] lines)
            {
                Lines = lines;
            }

            public string ErrorMessage { get; set; }

            public string[] Lines { get; }

            public event EventHandler<string> ErrorOccured;

            public void WithError(string message)
            {
                ErrorMessage = message;
            }

            public IEnumerable<string> GetLines()
            {
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    ErrorOccured?.Invoke(this, ErrorMessage);
                    return Enumerable.Empty<string>();
                }
                return Lines;
            }
        }

        #endregion Simulator
    }
}
