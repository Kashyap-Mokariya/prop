using Microsoft.Extensions.DependencyInjection;
using PropVivo.Application.Repositories;

namespace PropVivo.AzureStorage
{
    public static class ConfigureServiceExtensions
    {
        public static void AddInjectionAzureStorage(this IServiceCollection services)
        {
            services.AddScoped<IAzureStorage, AzureStorage>();
        }
    }
}