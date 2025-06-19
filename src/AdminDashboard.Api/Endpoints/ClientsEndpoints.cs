using AdminDashboard.Abstractions.Clients.Commands;
using AdminDashboard.Abstractions.Clients.Queries;
using AdminDashboard.Contracts.Clients;
using AdminDashboard.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboard.Api.Endpoints;

public static class ClientsEndpoints
{
    public static WebApplication AddClientsEndpointsGroup(this WebApplication app)
    {
        var clientsGroup = app.MapGroup("/clients").WithTags("Clients").RequireAuthorization().WithOpenApi();

        clientsGroup.MapGet("/{id:guid}",
                async Task<Results<Ok<ClientDto>, ProblemHttpResult, NotFound>> ([FromRoute] Guid id,
                    [FromServices] IGetClientByIdQuery query) =>
                {
                    try
                    {
                        var client = await query.GetByIdAsync(id);

                        if (client is null)
                        {
                            return TypedResults.NotFound();
                        }

                        return TypedResults.Ok(client);
                    }
                    catch (NotFoundException ex)
                    {
                        return TypedResults.Problem(
                            title: "Client not found",
                            detail: ex.Message,
                            statusCode: StatusCodes.Status404NotFound);
                    }
                })
            .WithName("GetClientById")
            .WithOpenApi();

        clientsGroup.MapGet("",
            async Task<Ok<List<ClientDto>>> (
                [FromServices] IGetClientsQuery query) =>
            {
                var clients = await query.GetClientsAsync();
                return TypedResults.Ok(clients);
            });

        clientsGroup.MapPost("",
                async Task<Results<CreatedAtRoute, ValidationProblem>> (
                    [FromBody] CreateClientRequest request,
                    [FromServices] ICreateClientCommand command,
                    [FromServices] IValidator<CreateClientRequest> validator) =>
                {
                    var validationResult = await validator.ValidateAsync(request);
                    if (!validationResult.IsValid)
                        return TypedResults.ValidationProblem(validationResult.ToDictionary());

                    var newClientId = await command.ExecuteAsync(request);
                    return TypedResults.CreatedAtRoute("GetClientById", new { id = newClientId });
                })
            .WithName("CreateClient")
            .WithOpenApi();

        clientsGroup.MapPut("/{id:guid}", async Task<Results<NoContent, ValidationProblem, ProblemHttpResult>> (
                [FromRoute] Guid id,
                [FromBody] UpdateClientRequest request,
                [FromServices] IUpdateClientCommand command,
                [FromServices] IValidator<UpdateClientRequest> validator) =>
            {
                request.Id = id;

                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                    return TypedResults.ValidationProblem(validationResult.ToDictionary());

                try
                {
                    await command.ExecuteAsync(request);
                    return TypedResults.NoContent();
                }
                catch (NotFoundException ex)
                {
                    return TypedResults.Problem(
                        title: "Client not found",
                        detail: ex.Message,
                        statusCode: StatusCodes.Status404NotFound);
                }
            })
            .WithName("UpdateClient")
            .WithOpenApi();


        clientsGroup.MapPost("/delete-multiple", async (
                [FromBody] DeleteClientsRequest request,
                [FromServices] IDeleteClientsCommand command) =>
            {
                try
                {
                    await command.ExecuteAsync(request);
                    return Results.NoContent();
                }
                catch (NotFoundException ex)
                {
                    return Results.Problem(
                        title: "Client not found",
                        detail: ex.Message,
                        statusCode: StatusCodes.Status404NotFound);
                }
            })
            .WithName("DeleteClient")
            .WithOpenApi();

        return app;
    }
}