using AdminDashboard.Contracts.Clients;

namespace AdminDashboard.Abstractions.Clients.Commands;

public interface IDeleteClientsCommand
{
    Task ExecuteAsync(DeleteClientsRequest request, CancellationToken cancellationToken = default);
}