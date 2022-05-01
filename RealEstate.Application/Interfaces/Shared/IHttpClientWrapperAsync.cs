namespace RealEstate.Application.Interfaces.Shared
{
    public interface IHttpClientWrapperAsync<T> where T : class
    {
        Task<T> GetAsync(string url);
    }
}
