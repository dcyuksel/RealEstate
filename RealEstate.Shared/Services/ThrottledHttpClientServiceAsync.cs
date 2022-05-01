using Newtonsoft.Json;
using RealEstate.Application.Interfaces.Shared;
using System.Collections.Concurrent;

namespace RealEstate.Shared.Services
{
    public class ThrottledHttpClientServiceAsync<T> : IThrottledHttpClientServiceAsync<T> where T : class
    {
        private readonly IHttpClientServiceAsync httpClientServiceAsync;
        private readonly RealEstateConfiguration realEstateConfiguration;

        public ThrottledHttpClientServiceAsync(
            IHttpClientServiceAsync httpClientServiceAsync,
            RealEstateConfiguration realEstateConfiguration)
        {
            this.httpClientServiceAsync = httpClientServiceAsync;
            this.realEstateConfiguration = realEstateConfiguration;
        }

        public async Task<IReadOnlyList<T>> GetAsync(IReadOnlyList<string> urls)
        {
            var semaphoreSlim = new SemaphoreSlim(
                  initialCount: realEstateConfiguration.HttpRequestInitialCount,
                  maxCount: realEstateConfiguration.HttpRequestMaxCount);
            var responses = new ConcurrentBag<T>();

            var i = 0;
            while (i < urls.Count)
            {
                var url = urls[i];
                await semaphoreSlim.WaitAsync();

                var response = await httpClientServiceAsync.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    await Task.Delay(1000);
                    semaphoreSlim.Release();
                    continue;
                }

                var content = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<T>(content);

                responses.Add(obj);
                i++;
                semaphoreSlim.Release();
            }

            return responses.ToArray();
        }
    }
}
