using Newtonsoft.Json;
using RealEstate.Application.Exceptions;
using RealEstate.Application.Interfaces.Shared;
using System.Collections.Concurrent;

namespace RealEstate.Shared.Services
{
    public class ThrottledHttpClientServiceAsync<T> : IThrottledHttpClientServiceAsync<T> where T : class
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly RealEstateConfiguration realEstateConfiguration;

        public ThrottledHttpClientServiceAsync(IHttpClientFactory httpClientFactory, RealEstateConfiguration realEstateConfiguration)
        {
            this.httpClientFactory = httpClientFactory;
            this.realEstateConfiguration = realEstateConfiguration;
        }

        public async Task<IReadOnlyList<T>> GetAsync(IReadOnlyList<string> urls)
        {
            var semaphore = new Semaphore(realEstateConfiguration.HttpRequestInitialCount, realEstateConfiguration.HttpRequestMaxCount, realEstateConfiguration.SemaphoreName);
            var httpClient = httpClientFactory.CreateClient(realEstateConfiguration.HttpClientName);
            var responses = new ConcurrentBag<HttpResponseMessage>();

            var tasks = urls.Select(async url =>
            {
                semaphore.WaitOne();
                var response = await httpClient.GetAsync(url);
                responses.Add(response);
                semaphore.Release();
            });

            await Task.WhenAll(tasks);

            if (responses.Any(response => !response.IsSuccessStatusCode))
            {
                throw new HttpRequestFailException("Http request is failed");
            }

            var result = new List<T>();
            foreach (var response in responses)
            {
                var content = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<T>(content);
                result.Add(obj);
            }

            return result;
        }

        public async Task<T> GetAsync(string url)
        {
            var semaphore = new Semaphore(realEstateConfiguration.HttpRequestInitialCount, realEstateConfiguration.HttpRequestMaxCount, realEstateConfiguration.SemaphoreName);
            var httpClient = httpClientFactory.CreateClient(realEstateConfiguration.HttpClientName);

            semaphore.WaitOne();
            var response = await httpClient.GetAsync(url);
            semaphore.Release();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestFailException("Http request is failed");
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
