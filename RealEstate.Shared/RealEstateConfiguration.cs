namespace RealEstate.Shared
{
    public class RealEstateConfiguration
    {
        public string RealEstateApiKey { get; set; }
        public int HttpRequestInitialCount { get; set; }
        public int HttpRequestMaxCount { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(RealEstateApiKey))
            {
                throw new ArgumentNullException(nameof(RealEstateApiKey));
            }

            if (HttpRequestInitialCount < 1)
            {
                throw new Exception($"{nameof(HttpRequestInitialCount)} could not be less than 1");
            }

            if (HttpRequestMaxCount < 1)
            {
                throw new Exception($"{nameof(HttpRequestMaxCount)} could not be less than 1");
            }
        }
    }
}
