using AdminDashboard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdminDashboard.Infrastructure.Extensions;

public static class ContextInjector
{
    public static IServiceCollection AddAdminDashboardContext(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<DbContextOptionsBuilder>? configureOptions = null,
        bool usePooling = true,
        int poolSize = 128,
        string connectionName = "PostgresConnection")
    {
        var connectionString = configuration.GetConnectionString(connectionName);

        if (usePooling)
        {
            services.AddDbContextPool<AppDbContext>(options =>
            {
                configureOptions?.Invoke(options);
                ApplyDefaultConfiguration(options, connectionString);
            }, poolSize);
        }
        else
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                configureOptions?.Invoke(options);
                ApplyDefaultConfiguration(options, connectionString);
            });
        }

        return services;
    }

    private static void ApplyDefaultConfiguration(
        DbContextOptionsBuilder options,
        string connectionString)
    {
        options
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention()
            .ConfigureWarnings(warnings => { warnings.Ignore(RelationalEventId.PendingModelChangesWarning); });
        ;
    }
}