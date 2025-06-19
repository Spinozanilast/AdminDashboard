using AdminDashboard.Api.Endpoints;
using AdminDashboard.Api.Middlewares;
using AdminDashboard.Infrastructure;
using AdminDashboard.Application;
using AdminDashboard.Infrastructure.Extensions;
using Asp.Versioning;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new()
        {
            Title = "My API",
            Version = "v1",
            Description = "API Documentation using Scalar v2",
            Contact = new()
            {
                Name = "Support Team",
                Email = "support@myapi.com"
            }
        });

        var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
    });

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

builder.Services.AddCors(options =>
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()));

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
    app.UseSwagger(opt =>
    {
        opt.RouteTemplate = "openapi/{documentName}.json";
    });
    app.UseSwaggerUI();
    app.MapScalarApiReference(opt =>
    {
        opt.Title = "Scalar Example";
        opt.Theme = ScalarTheme.Mars;
        opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
    });

    await app.ApplyMigrations();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();