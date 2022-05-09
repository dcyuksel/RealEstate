using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using RealEstate.Application;
using RealEstate.Shared;
using RealEstate.WebApi.Middlewares;

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

            services.AddApplicationServices();
            services.AddSharedServices();


            var configuration = Configuration.Get<RealEstateConfiguration>();
            configuration.Validate();
            services.AddSingleton(configuration);

            //AddConfiguration(services);
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

        //private void AddConfiguration(IServiceCollection services)
        //{
        //    var realEstateHttpApiKey = Configuration.GetValue<string>("RealEstateApiKey");
        //    var httpRequestInitialCount = Configuration.GetValue<int>("HttpRequestInitialCount");
        //    var httpRequestMaxCount = Configuration.GetValue<int>("HttpRequestMaxCount");
        //    var realEstateConfiguration = new RealEstateConfiguration(realEstateHttpApiKey, httpRequestInitialCount, httpRequestMaxCount);
        //    services.AddSingleton(realEstateConfiguration);
        //}
    }
}
