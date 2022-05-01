using RealEstate.Application.Interfaces.Shared;

namespace RealEstate.Shared.Services
{
    public class UrlService : IUrlService
    {
        private readonly RealEstateConfiguration realEstateConfiguration;

        public UrlService(RealEstateConfiguration realEstateConfiguration)
        {
            this.realEstateConfiguration = realEstateConfiguration;
        }

        public string GetRealEstateAgentUrl(int pageNumber, int pageSize)
        {
            return $"http://partnerapi.funda.nl/feeds/Aanbod.svc/json/{realEstateConfiguration.ApiKey}/?type=koop&zo=/amsterdam/makelaar/&page={pageNumber}&pagesize = {pageSize}";
        }

        public string GetRealEstateAgentWithGardenUrl(int pageNumber, int pageSize)
        {
            return $"http://partnerapi.funda.nl/feeds/Aanbod.svc/json/{realEstateConfiguration.ApiKey}/?type=koop&zo=/amsterdam/tuin/&page={pageNumber}&pagesize = {pageSize}";
        }
    }
}
