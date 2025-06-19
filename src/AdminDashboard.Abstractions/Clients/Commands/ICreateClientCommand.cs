using AdminDashboard.Contracts.Clients;

namespace AdminDashboard.Abstractions.Clients.Commands;

public interface ICreateClientCommand
{
    Task<Guid> ExecuteAsync(CreateClientRequest request, CancellationToken cancellationToken = default);
}