using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using RealEstate.Application;
using RealEstate.Shared;
using RealEstate.WebApi.Middlewares;
using Polly;
using System;
using Polly.Extensions.Http;
using System.Net;

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

            var configuration = Configuration.Get<RealEstateConfiguration>();
            configuration.Validate();
            services.AddSingleton(configuration);

            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(response => response.StatusCode != HttpStatusCode.OK)
                .WaitAndRetryAsync(configuration.RetryCountPerRequest, retryAttempt => TimeSpan.FromSeconds(retryAttempt));

            services.AddHttpClient(configuration.HttpClientName).AddPolicyHandler(retryPolicy);

            services.AddApplicationServices();
            services.AddSharedServices();
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
