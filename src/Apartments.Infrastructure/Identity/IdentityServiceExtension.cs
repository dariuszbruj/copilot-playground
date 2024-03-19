using Apartments.Application.Modules.AccountServices;
using Apartments.Application.Modules.Tokens;
using Apartments.Infrastructure.EntityFramework.Contexts;
using Apartments.Infrastructure.Identity.Models;
using Apartments.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Apartments.Infrastructure.Identity;

public static class IdentityServiceExtension
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityCore<User>(options => { options.User.RequireUniqueEmail = true; })
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddSignInManager();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["JwtTokenGeneratorOptions:Audience"],
                    ValidIssuer = configuration["JwtTokenGeneratorOptions:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtTokenGeneratorOptions:Key"]))
                };
            });
        services.AddAuthorization();
            
        services.AddSingleton(TimeProvider.System);
        services.Configure<JwtTokenGeneratorOptions>(configuration.GetSection("JwtTokenGeneratorOptions"));
        services.AddTransient<ITokenGenerator, JwtTokenGenerator>();

        services.AddTransient<IAccountService, AccountService>();
        
        return services;
    }
}
