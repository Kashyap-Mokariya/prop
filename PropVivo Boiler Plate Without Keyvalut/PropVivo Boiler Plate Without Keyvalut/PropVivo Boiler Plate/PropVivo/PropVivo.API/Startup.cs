using Asp.Versioning;
using PropVivo.API.Extensions;
using PropVivo.Application;
using PropVivo.AzureStorage;
using PropVivo.Infrastructure;

namespace PropVivo.API
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();

            app.UseSwaggerGen();
            app.EnsureCosmosDbIsCreated();
            app.AddMiddleware();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // app.UseCors("CorsPolicy");
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            services.AddInjectionApplication();
            services.AddInjectionAzureStorage();
            services.AddInjectionPersistence(configuration);

            services.ConfigureApiBehavior();
            services.ConfigureCorsPolicy();

            services.AddSwaggerDocumentation();
            services.AddIdentityService(configuration);
            //services.AddHostedService<WorkerServiceBus>();

            // Add versioning in the APIs
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
            });
        }
    }
}