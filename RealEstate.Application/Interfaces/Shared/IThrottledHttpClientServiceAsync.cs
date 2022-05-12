namespace RealEstate.Application.Interfaces.Shared
{
    public interface IThrottledHttpClientServiceAsync<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAsync(IReadOnlyList<string> urls);

        Task<T> GetAsync(string url);
    }
}
