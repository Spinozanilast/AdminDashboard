using AdminDashboard.Contracts.Clients;

namespace AdminDashboard.Abstractions.Clients.Commands;

public interface IUpdateClientCommand
{
    Task ExecuteAsync(UpdateClientRequest request, CancellationToken cancellationToken = default);
}