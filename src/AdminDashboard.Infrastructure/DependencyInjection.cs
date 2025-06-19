using System.Security.Claims;
using System.Text;
using AdminDashboard.Abstractions;
using AdminDashboard.Abstractions.Auth;
using AdminDashboard.Abstractions.Clients;
using AdminDashboard.Abstractions.ExchangeRates;
using AdminDashboard.Abstractions.Payments;
using AdminDashboard.Abstractions.Tags;
using AdminDashboard.Infrastructure.Data;
using AdminDashboard.Infrastructure.Data.Repositories;
using AdminDashboard.Infrastructure.Extensions;
using AdminDashboard.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AdminDashboard.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

        services.AddAdminDashboardContext(configuration);

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])),
                    ValidateIssuer = configuration.GetValue<bool>("Jwt:ValidateIssuer"),
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateAudience = configuration.GetValue<bool>("Jwt:ValidateAudience"),
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateLifetime = configuration.GetValue<bool>("Jwt:ValidateLifetime"),
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception is SecurityTokenExpiredException)
                        {
                            context.Response.Headers.Append("Token-Expired", "true");
                        }

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        var userId = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);

                        if (userId != null)
                        {
                            context.HttpContext.Items["UserId"] = userId;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddScoped<IClientsRepository, ClientsRepository>();
        services.AddScoped<IExchangeRatesRepository, ExchangeRatesRepository>();
        services.AddScoped<IPaymentsRepository, PaymentsRepository>();
        services.AddScoped<IRefreshTokensRepository, RefreshTokenRepository>();
        services.AddScoped<ITagsRepository, TagsRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<IJwtService, JwtService>();

        return services;
    }
}