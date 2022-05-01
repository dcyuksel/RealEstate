using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using RealEstate.Application.Interfaces.Shared;
using RealEstate.Shared.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Shared.UnitTests
{
    public class ThrottledHttpClientServiceAsyncUnitTests
    {
        private readonly string Url = "http://localhost:1234";
        private readonly IEnumerable<string> SampleData = new List<string> { "real", "estate" };
        private Mock<IHttpClientServiceAsync> mockHttpClientService;

        [SetUp]
        public void Setup()
        {
            var json = JsonConvert.SerializeObject(SampleData);

            HttpResponseMessage httpResponse = new HttpResponseMessage();
            httpResponse.StatusCode = System.Net.HttpStatusCode.OK;
            httpResponse.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var mockHttpClientService = new Mock<IHttpClientServiceAsync>();
            mockHttpClientService.Setup(t => t.GetAsync(It.Is<string>(s => s.StartsWith(Url)))).ReturnsAsync(httpResponse);

            this.mockHttpClientService = mockHttpClientService;
        }

        [Test]
        public async Task GetAsyncTest()
        {
            var urls = new List<string> { Url, Url, Url };
            var throttledHttpClientServiceAsync = new ThrottledHttpClientServiceAsync<IEnumerable<string>>(mockHttpClientService.Object, new RealEstateConfiguration("key", 10, 10));
            var result = await throttledHttpClientServiceAsync.GetAsync(urls);
            Assert.AreEqual(urls.Count, result.Count);
        }
    }
}
