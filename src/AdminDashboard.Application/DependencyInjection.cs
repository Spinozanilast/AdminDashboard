using AdminDashboard.Abstractions.Auth.Commands;
using AdminDashboard.Abstractions.Clients.Commands;
using AdminDashboard.Abstractions.Clients.Queries;
using AdminDashboard.Abstractions.ExchangeRates.Commands;
using AdminDashboard.Abstractions.ExchangeRates.Queries;
using AdminDashboard.Abstractions.Payments.Queries;
using AdminDashboard.Application.Auth.Commands;
using AdminDashboard.Application.Clients.Commands;
using AdminDashboard.Application.Clients.Queries;
using AdminDashboard.Application.Clients.Validators;
using AdminDashboard.Application.ExchangeRates.Commands;
using AdminDashboard.Application.ExchangeRates.Queries;
using AdminDashboard.Application.Payments.Queries;
using AdminDashboard.Contracts.Clients;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AdminDashboard.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddAuthFeature()
            .AddClientsFeature()
            .AddValidators()
            .AddExchangeRatesFeature()
            .AddPaymentsFeature();

        return services;
    }

    private static IServiceCollection AddAuthFeature(this IServiceCollection services)
    {
        services.AddTransient<ILoginCommand, LoginCommand>();
        services.AddTransient<IRefreshTokenCommand, RefreshTokenCommand>();

        return services;
    }

    private static IServiceCollection AddClientsFeature(this IServiceCollection services)
    {
        services.AddTransient<ICreateClientCommand, CreateClientCommand>();
        services.AddTransient<IDeleteClientsCommand, DeleteClientsCommand>();
        services.AddTransient<IUpdateClientCommand, UpdateClientCommand>();

        services.AddTransient<IGetClientsQuery, GetClientsQuery>();

        return services;
    }

    private static IServiceCollection AddExchangeRatesFeature(this IServiceCollection services)
    {
        services.AddTransient<IUpdateExchangeRateCommand, UpdateExchangeRateCommand>();

        services.AddTransient<IGetExchangeRateQuery, GetExchangeRateQuery>();

        return services;
    }

    private static IServiceCollection AddPaymentsFeature(this IServiceCollection services)
    {
        services.AddTransient<IGetPaymentsQuery, GetPaymentsQuery>();

        return services;
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateClientRequest>, CreateClientCommandValidator>();
        services.AddScoped<IValidator<UpdateClientRequest>, UpdateClientCommandValidator>();

        return services;
    }
}