using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PropVivo.Application.Repositories;
using PropVivo.Application.Services;
using PropVivo.Infrastructure.AppSettings;
using PropVivo.Infrastructure.Contexts;
using PropVivo.Infrastructure.Repositories;
using PropVivo.Infrastructure.Services;

namespace PropVivo.Infrastructure
{
    public static class ConfigureServiceExtensions
    {
        public static void AddInjectionPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var cosmosDbConfig = configuration.GetSection("ConnectionStrings:CosmosDB").Get<CosmosDbSettings>();
            services.AddCosmosDb(cosmosDbConfig);

            services.AddScoped<DapperContext>();
            services.AddScoped<ISqlRepository, SqlRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IServiceRequestRepository, ServiceRequestRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICallRepository, CallRepository>();
            services.AddScoped<ICallNotificationService, CallNotificationService>();
            services.AddHttpClient<IVoiceModulationService, VoiceModulationService>();
            services.AddScoped<IVoiceModulationService, VoiceModulationService>();
        }
    }
}
