using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace PropVivo.API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services,
            IConfiguration _configuration)
        {
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                    //ValidateIssuerSigningKey = true,
                    //ValidIssuer = _configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(_configuration["Jwt:Key"]))
                };
            });
            return services;
        }
    }
}