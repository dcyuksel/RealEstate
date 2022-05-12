using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using RealEstate.Shared.Services;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RealEstate.Shared.UnitTests
{
    public class ThrottledHttpClientServiceAsyncUnitTests
    {
        private readonly string Url = "http://localhost:1234";
        private readonly IList<string> SampleData = new List<string> { "real", "estate" };
        private Mock<IHttpClientFactory> mockHttpClientFactory;

        [SetUp]
        public void Setup()
        {
            var json = JsonConvert.SerializeObject(SampleData);

            var clientHandlerStub = new DelegatingHandlerStub((request, cancellationToken) => {
                var response = new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, Content = new StringContent(json) };
                return Task.FromResult(response);
            });

            var factoryMock = new Mock<IHttpClientFactory>();
            factoryMock.Setup(m => m.CreateClient(It.IsAny<string>()))
                .Returns(() => new HttpClient(clientHandlerStub));

            mockHttpClientFactory = factoryMock;
        }

        [Test]
        public async Task MultipleGetAsyncTest()
        {
            var urls = new List<string> { Url, Url, Url };
            var throttledHttpClientServiceAsync = new ThrottledHttpClientServiceAsync<IList<string>>(mockHttpClientFactory.Object, new RealEstateConfiguration { RealEstateApiKey = "key", HttpRequestInitialCount = 10, HttpRequestMaxCount = 10 });
            var result = await throttledHttpClientServiceAsync.GetAsync(urls);
            Assert.AreEqual(urls.Count, result.Count);
        }

        [Test]
        public async Task SingleGetAsyncTest()
        {
            var throttledHttpClientServiceAsync = new ThrottledHttpClientServiceAsync<IList<string>>(mockHttpClientFactory.Object, new RealEstateConfiguration { RealEstateApiKey = "key", HttpRequestInitialCount = 10, HttpRequestMaxCount = 10 });
            var result = await throttledHttpClientServiceAsync.GetAsync(Url);
            Assert.AreEqual(SampleData.Count, result.Count);
            Assert.AreEqual(SampleData[0], result[0]);
            Assert.AreEqual(SampleData[1], result[1]);
        }
    }
}
