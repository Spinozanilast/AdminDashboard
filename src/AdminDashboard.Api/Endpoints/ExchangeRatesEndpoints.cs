using AdminDashboard.Abstractions.ExchangeRates.Commands;
using AdminDashboard.Abstractions.ExchangeRates.Queries;
using AdminDashboard.Contracts.ExchangeRates;
using AdminDashboard.Domain.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Api.Endpoints;

public static class ExchangeRatesEndpoints
{
    public static WebApplication AddExchangeRatesEndpointsGroup(this WebApplication app)
    {
        var ratesGroup = app.MapGroup("/rates").WithTags("Rates").RequireAuthorization().WithOpenApi();

        ratesGroup.MapGet("", async Task<Results<Ok<ExchangeRateDto>, ProblemHttpResult>> (
                [FromServices] IGetExchangeRateQuery query) =>
            {
                try
                {
                    var rate = await query.GetExchangeRateAsync(new GetExchangeRateRequest());
                    return TypedResults.Ok(rate);
                }
                catch (NotFoundException ex)
                {
                    return TypedResults.Problem(
                        title: "Exchange rate not found",
                        detail: ex.Message,
                        statusCode: StatusCodes.Status404NotFound);
                }
            })
            .WithName("GetExchangeRate")
            .WithOpenApi();

        ratesGroup.MapPost("", async Task<Results<NoContent, ProblemHttpResult>> (
                [FromBody] UpdateExchangeRateRequest request,
                [FromServices] IUpdateExchangeRateCommand command) =>
            {
                if (request.NewRate < 0)
                {
                    return TypedResults.Problem(title: "Exchange rate must be positive",
                        statusCode: StatusCodes.Status400BadRequest);
                }

                await command.ExecuteAsync(request);
                return TypedResults.NoContent();
            })
            .WithName("UpdateExchangeRate")
            .WithOpenApi();

        return app;
    }
}