namespace RealEstate.Shared
{
    public class RealEstateConfiguration
    {
        public RealEstateConfiguration(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException(nameof(apiKey));
            }

            ApiKey = apiKey;
        }

        public string ApiKey { get; }
    }
}
