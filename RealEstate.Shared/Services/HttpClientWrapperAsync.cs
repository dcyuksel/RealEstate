using Newtonsoft.Json;
using RealEstate.Application.Interfaces.Shared;

namespace RealEstate.Shared.Services
{
    public class HttpClientWrapperAsync<T> : IHttpClientWrapperAsync<T> where T : class
    {
        private readonly IHttpClientFactory httpClientFactory;

        public HttpClientWrapperAsync(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<T> GetAsync(string url)
        {
            T result = null;

            var httpClient = httpClientFactory.CreateClient("funda");
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
            {
                if (x.IsFaulted)
                    throw x.Exception;

                result = JsonConvert.DeserializeObject<T>(x.Result);
            });

            return result;
        }
    }
}
