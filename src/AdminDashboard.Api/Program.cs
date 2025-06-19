using AdminDashboard.Api.Endpoints;
using AdminDashboard.Api.Middlewares;
using AdminDashboard.Infrastructure;
using AdminDashboard.Application;
using AdminDashboard.Infrastructure.Extensions;
using Asp.Versioning;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });

var app = builder.Build();

app.AddAuthEndpointsGroup()
    .AddClientsEndpointsGroup()
    .AddExchangeRatesEndpointsGroup()
    .AddPaymentsEndpointsGroup();

// For Api Versioning 
// var apiVersionSet = app.NewApiVersionSet()
//     .HasApiVersion(new ApiVersion(1, 0))
//     .ReportApiVersions()
//     .Build();
//
// var defaultApiVersion = new ApiVersion(1, 0);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.ApplyMigrations();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();