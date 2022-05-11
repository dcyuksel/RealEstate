using Microsoft.Extensions.DependencyInjection;
using RealEstate.Application.Interfaces.Shared;
using RealEstate.Shared.Services;

namespace RealEstate.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedServices(this IServiceCollection services)
        {
            services.AddTransient<IUrlService, UrlService>();
            services.AddTransient(typeof(IHttpClientWrapperAsync<>), typeof(HttpClientWrapperAsync<>));
            services.AddTransient(typeof(IThrottledHttpClientServiceAsync<>), typeof(ThrottledHttpClientServiceAsync<>));
        }
    }
}
