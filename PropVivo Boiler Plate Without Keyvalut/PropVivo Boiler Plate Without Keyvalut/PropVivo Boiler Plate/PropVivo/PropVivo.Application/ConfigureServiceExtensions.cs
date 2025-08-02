using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PropVivo.Application.Common.Behaviors;
using System.Reflection;

namespace PropVivo.Application
{
    public static class ConfigureServiceExtensions
    {
        public static void AddInjectionApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddSingleton<ClaimsPrincipalExtensions>();
            services.AddHttpContextAccessor();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        }
    }
}