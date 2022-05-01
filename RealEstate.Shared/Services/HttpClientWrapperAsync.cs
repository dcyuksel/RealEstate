using Newtonsoft.Json;
using RealEstate.Application.Interfaces.Shared;

namespace RealEstate.Shared.Services
{
    public class HttpClientWrapperAsync<T> : IHttpClientWrapperAsync<T> where T : class
    {
        private readonly IHttpClientServiceAsync httpClientService;

        public HttpClientWrapperAsync(IHttpClientServiceAsync httpClientService)
        {
            this.httpClientService = httpClientService;
        }

        public async Task<T> GetAsync(string url)
        {
            T result = null;

            var response = await httpClientService.GetAsync(url);
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
