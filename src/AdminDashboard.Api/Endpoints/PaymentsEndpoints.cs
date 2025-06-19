using AdminDashboard.Abstractions.Payments.Queries;
using AdminDashboard.Contracts.Payments;
using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Api.Endpoints;

public static class PaymentsEndpoints
{
    public static WebApplication AddPaymentsEndpointsGroup(this WebApplication app)
    {
        var paymentsGroup = app.MapGroup("/payments").WithTags("Payments").RequireAuthorization();

        paymentsGroup.MapGet("", async Task<Ok<List<PaymentDto>>> (
                [FromQuery] int? take,
                [FromServices] IGetPaymentsQuery query) =>
            {
                var definedQuery = new GetPaymentsRequest(take ?? 5);
                var payments = await query.GetPaymentsAsync(definedQuery);
                return TypedResults.Ok(payments);
            })
            .Produces<List<PaymentDto>>()
            .WithName("GetPayments")
            .WithOpenApi();

        return app;
    }
}