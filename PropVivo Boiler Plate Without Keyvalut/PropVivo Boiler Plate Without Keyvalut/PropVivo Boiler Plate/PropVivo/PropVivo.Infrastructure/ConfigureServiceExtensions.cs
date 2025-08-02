using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Enums;
using PropVivo.Infrastructure.AppSettings;
using PropVivo.Infrastructure.Contexts;
using PropVivo.Infrastructure.Repositories;

namespace PropVivo.Infrastructure
{
    public static class ConfigureServiceExtensions
    {
        public static void AddInjectionPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null || configuration.GetSection("ConnectionStrings:CosmosDB") == null)
                return;

            // Bind database-related bindings
            CosmosDbSettings cosmosDbConfig = configuration.GetSection("ConnectionStrings:CosmosDB").Get<CosmosDbSettings>();
            if (cosmosDbConfig == null)
                return;

            // register CosmosDB client and data repositories
            services.AddCosmosDb(cosmosDbConfig);

            services.AddSingleton<DapperContext>();
            services.AddScoped<ISqlRepository, SqlRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
        }
    }
}