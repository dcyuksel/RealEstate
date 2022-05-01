namespace RealEstate.Shared
{
    public class RealEstateConfiguration
    {
        public RealEstateConfiguration(
            string apiKey,
            int httpRequestInitialCount,
            int httpRequestMaxCount)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException(nameof(apiKey));
            }

            if (httpRequestInitialCount < 1)
            {
                throw new Exception($"{nameof(httpRequestInitialCount)} could not be less than 1");
            }

            if (httpRequestMaxCount < 1)
            {
                throw new Exception($"{nameof(httpRequestMaxCount)} could not be less than 1");
            }

            ApiKey = apiKey;
            HttpRequestInitialCount = httpRequestInitialCount;
            HttpRequestMaxCount = httpRequestMaxCount;
        }

        public string ApiKey { get; }
        public int HttpRequestInitialCount { get; }
        public int HttpRequestMaxCount { get; }
    }
}
