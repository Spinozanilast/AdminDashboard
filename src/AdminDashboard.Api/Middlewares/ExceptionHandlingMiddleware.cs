using System.Text.Json;
using AdminDashboard.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var problemDetails = new ProblemDetails();

        switch (exception)
        {
            case ValidationException vex:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Validation error";
                problemDetails.Detail = vex.Message;
                problemDetails.Extensions["errors"] = vex.Errors;
                break;

            case UnauthorizedAccessException _:
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                problemDetails.Title = "Unauthorized";
                problemDetails.Detail = "Authentication failed";
                break;

            case NotFoundException nfex:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                problemDetails.Title = "Not Found";
                problemDetails.Detail = nfex.Message;
                break;

            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                problemDetails.Title = "Server Error";
                problemDetails.Detail = _env.IsDevelopment()
                    ? exception.Message
                    : "An unexpected error occurred";
                break;
        }

        problemDetails.Status = context.Response.StatusCode;
        problemDetails.Instance = context.Request.Path;

        if (_env.IsDevelopment())
        {
            problemDetails.Extensions["trace"] = exception.StackTrace;
        }

        var json = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });

        await context.Response.WriteAsync(json);
    }
}