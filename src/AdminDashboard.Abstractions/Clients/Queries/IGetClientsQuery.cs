using AdminDashboard.Contracts.Clients;

namespace AdminDashboard.Abstractions.Clients.Queries;

public interface IGetClientsQuery
{
    Task<List<ClientDto>> GetClientsAsync(CancellationToken cancellationToken = default);
}