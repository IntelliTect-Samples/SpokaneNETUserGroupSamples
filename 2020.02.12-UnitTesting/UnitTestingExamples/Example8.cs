using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTestingExamples
{
    [TestClass]
    public class Example8
    {
        #region SUT

        public interface ILineSource
        {
            Task<IEnumerable<string>> GetLinesAsync();
        }

        public class WebLineSource : ILineSource
        {
            public WebLineSource(Uri uri, IHttpClientFactory httpClientFactory)
            {
                Uri = uri ?? throw new ArgumentNullException(nameof(uri));
                HttpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            }

            private Uri Uri { get; }
            private IHttpClientFactory HttpClientFactory { get; }

            public async Task<IEnumerable<string>> GetLinesAsync()
            {
                HttpClient httpClient = HttpClientFactory.CreateClient();
                try
                {
                    string data = await httpClient.GetStringAsync(Uri);
                    return data.Split('\n');
                }
                catch (HttpRequestException)
                {
                    return Enumerable.Empty<string>();
                }
            }
        }

        #endregion SUT

        #region Tests

        public TestContext TestContext { get; set; }

        [TestMethod]
        public async Task CanRetrieveValidData()
        {
            // Arrange
            var mocker = new AutoMocker().WithHttpClient();
            var messageHandler = mocker.GetMock<IHttpMessageHandler>();

            messageHandler.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    Content = new StringContent("42\n43")
                });

            mocker.Use(new Uri("https://testserver.gov/"));

            var webLineSource = mocker.CreateInstance<WebLineSource>();

            // Act
            string[] lines = (await webLineSource.GetLinesAsync()).ToArray();

            // Assert
            CollectionAssert.AreEqual(new[] { "42", "43" }, lines);
        }

        #endregion Tests
    }

    public interface IHttpMessageHandler
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
    }

    public static class AutoMockerHttpMixins
    {
        public static AutoMocker WithHttpClient(this AutoMocker mocker)
        {
            mocker.Setup<IHttpClientFactory, HttpClient>(x => x.CreateClient(It.IsAny<string>()))
                .Returns(() =>
                {
                    var messageHandler = mocker.Get<IHttpMessageHandler>();
                    var httpMessageHandler = new ForwardingMessageHandler(messageHandler);
                    return new HttpClient(httpMessageHandler);
                });

            return mocker;
        }

        private class ForwardingMessageHandler : HttpMessageHandler
        {
            public ForwardingMessageHandler(IHttpMessageHandler messageHandler)
            {
                MessageHandler = messageHandler ?? throw new ArgumentNullException(nameof(messageHandler));
            }

            public IHttpMessageHandler MessageHandler { get; }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                CancellationToken cancellationToken)
                => MessageHandler.SendAsync(request, cancellationToken);
        }
    }
}
