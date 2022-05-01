namespace RealEstate.Application.Interfaces.Shared
{
    public interface IUrlService
    {
        string GetRealEstateAgentUrl(int pageNumber, int pageSize);
        string GetRealEstateAgentWithGardenUrl(int pageNumber, int pageSize);
    }
}
