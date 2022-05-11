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
    public class HttpClientWrapperAsyncUnitTests
    {
        private readonly string Url = "http://localhost:1234";
        private readonly IEnumerable<string> SampleData = new List<string> { "real", "estate" };
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
        public async Task GetAsyncTest()
        {
            var httpClientWrapperAsync = new HttpClientWrapperAsync<IEnumerable<string>>(mockHttpClientFactory.Object);
            var result = await httpClientWrapperAsync.GetAsync(Url);
            Assert.AreEqual(SampleData, result);
        }
    }
}
