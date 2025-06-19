using AdminDashboard.Abstractions.Auth.Commands;
using AdminDashboard.Contracts.Auth;
using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Api.Endpoints;

public static class AuthEndpoints
{
    public static WebApplication AddAuthEndpointsGroup(this WebApplication app)
    {
        var authGroup = app.MapGroup("/auth").WithTags("Authentication");

        authGroup.MapPost("/login",
                async Task<Results<Ok<AuthResponse>, ProblemHttpResult>> (
                    [FromBody] LoginRequest request,
                    [FromServices] ILoginCommand loginCommand) =>
                {
                    try
                    {
                        var response = await loginCommand.ExecuteAsync(request);
                        return TypedResults.Ok(response);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        return TypedResults.Problem(
                            title: "Authentication failed",
                            detail: ex.Message,
                            statusCode: StatusCodes.Status401Unauthorized);
                    }
                })
            .WithName("Login")
            .WithOpenApi();

        authGroup.MapPost("/refresh",
                async Task<Results<Ok<AuthResponse>, ProblemHttpResult>> (
                    [FromBody] RefreshTokenRequest request,
                    [FromServices] IRefreshTokenCommand refreshTokenCommand) =>
                {
                    try
                    {
                        var response = await refreshTokenCommand.ExecuteAsync(request);
                        return TypedResults.Ok(response);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        return TypedResults.Problem(
                            title: "Token refresh failed",
                            detail: ex.Message,
                            statusCode: StatusCodes.Status401Unauthorized);
                    }
                })
            .WithName("RefreshToken")
            .WithOpenApi();

        return app;
    }
}