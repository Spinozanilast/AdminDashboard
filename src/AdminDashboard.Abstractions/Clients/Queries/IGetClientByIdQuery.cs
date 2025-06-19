using AdminDashboard.Contracts.Clients;

namespace AdminDashboard.Abstractions.Clients.Queries;

public interface IGetClientByIdQuery
{
    Task<ClientDto?> GetByIdAsync(Guid id);
}