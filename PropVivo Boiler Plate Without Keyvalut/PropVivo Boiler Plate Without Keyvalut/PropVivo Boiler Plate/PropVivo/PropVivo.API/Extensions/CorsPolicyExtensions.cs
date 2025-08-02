namespace PropVivo.API.Extensions
{
    public static class CorsPolicyExtensions
    {
        public static void ConfigureCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("CorsPolicy", policy =>
            //    {
            //        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
            //    });
            //});
        }
    }
}