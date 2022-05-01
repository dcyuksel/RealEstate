namespace RealEstate.Application.Interfaces.Shared
{
    public interface IHttpClientServiceAsync
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }
}
