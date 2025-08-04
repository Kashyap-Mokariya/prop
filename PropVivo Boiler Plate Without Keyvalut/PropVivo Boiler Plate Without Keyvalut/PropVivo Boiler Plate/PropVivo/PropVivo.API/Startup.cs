using Asp.Versioning;
using PropVivo.API.Extensions;
using PropVivo.API.Hubs;
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
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<CallHub>("/callhub");
            });
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSignalR();

            services.AddInjectionApplication();
            services.AddInjectionAzureStorage();
            services.AddInjectionPersistence(configuration);

            services.ConfigureApiBehavior();
            services.ConfigureCorsPolicy();
            services.AddSwaggerDocumentation();
            services.AddIdentityService(configuration);

            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
            });
        }
    }
}
