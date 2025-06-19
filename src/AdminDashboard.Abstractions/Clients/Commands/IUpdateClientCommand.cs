using AdminDashboard.Contracts.Clients;
using AdminDashboard.Domain;

namespace AdminDashboard.Abstractions.Clients.Commands;

public interface IUpdateClientCommand
{
    Task ExecuteAsync(UpdateClientRequest request, CancellationToken cancellationToken = default);
}