using NUnit.Framework;
using RealEstate.Shared.Services;

namespace RealEstate.Shared.UnitTests
{
    public class UrlServiceUnitTests
    {
        [Test]
        [TestCase(1, 15, "apiKey1", "http://partnerapi.funda.nl/feeds/Aanbod.svc/json/apiKey1/?type=koop&zo=/amsterdam/makelaar/&page=1&pagesize = 15")]
        [TestCase(2, 15, "apiKey2", "http://partnerapi.funda.nl/feeds/Aanbod.svc/json/apiKey2/?type=koop&zo=/amsterdam/makelaar/&page=2&pagesize = 15")]
        [TestCase(3, 25, "apiKey3", "http://partnerapi.funda.nl/feeds/Aanbod.svc/json/apiKey3/?type=koop&zo=/amsterdam/makelaar/&page=3&pagesize = 25")]
        [TestCase(1000, 15, "apiKey4", "http://partnerapi.funda.nl/feeds/Aanbod.svc/json/apiKey4/?type=koop&zo=/amsterdam/makelaar/&page=1000&pagesize = 15")]
        public void GetRealEstateAgentUrlTest(int pageNumber, int pageSize, string apiKey, string expectedResult)
        {
            var urlService = new UrlService(new RealEstateConfiguration { RealEstateApiKey = apiKey, HttpRequestInitialCount = 10, HttpRequestMaxCount = 10 });
            var result = urlService.GetRealEstateAgentUrl(pageNumber, pageSize);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase(1, 15, "apiKey1", "http://partnerapi.funda.nl/feeds/Aanbod.svc/json/apiKey1/?type=koop&zo=/amsterdam/tuin/&page=1&pagesize = 15")]
        [TestCase(2, 15, "apiKey2", "http://partnerapi.funda.nl/feeds/Aanbod.svc/json/apiKey2/?type=koop&zo=/amsterdam/tuin/&page=2&pagesize = 15")]
        [TestCase(3, 25, "apiKey3", "http://partnerapi.funda.nl/feeds/Aanbod.svc/json/apiKey3/?type=koop&zo=/amsterdam/tuin/&page=3&pagesize = 25")]
        [TestCase(1000, 15, "apiKey4", "http://partnerapi.funda.nl/feeds/Aanbod.svc/json/apiKey4/?type=koop&zo=/amsterdam/tuin/&page=1000&pagesize = 15")]
        public void GetRealEstateAgentWithGardenUrlTest(int pageNumber, int pageSize, string apiKey, string expectedResult)
        {
            var urlService = new UrlService(new RealEstateConfiguration { RealEstateApiKey = apiKey, HttpRequestInitialCount = 10, HttpRequestMaxCount = 10 });
            var result = urlService.GetRealEstateAgentWithGardenUrl(pageNumber, pageSize);
            Assert.AreEqual(expectedResult, result);
        }
    }
}
