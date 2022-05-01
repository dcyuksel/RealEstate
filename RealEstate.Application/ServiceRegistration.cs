using MediatR;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace RealEstate.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
