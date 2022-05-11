using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using RealEstate.Application;
using RealEstate.Shared;
using RealEstate.WebApi.Middlewares;
using Microsoft.Extensions.Http;
using Polly;
using System.Net.Http;
using System;
using Polly.Extensions.Http;
using RealEstate.Application.Interfaces.Shared;
using RealEstate.Shared.Services;

namespace RealEstate.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RealEstate.WebApi", Version = "v1" });
            });

            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt));

            services.AddHttpClient("funda").AddPolicyHandler(retryPolicy);

            services.AddApplicationServices();
            services.AddSharedServices();

            var configuration = Configuration.Get<RealEstateConfiguration>();
            configuration.Validate();
            services.AddSingleton(configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RealEstate.WebApi v1"));

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
