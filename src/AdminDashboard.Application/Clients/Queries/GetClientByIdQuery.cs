using AdminDashboard.Abstractions.Clients;
using AdminDashboard.Abstractions.Clients.Queries;
using AdminDashboard.Contracts.Clients;
using AdminDashboard.Contracts.Tags;

namespace AdminDashboard.Application.Clients.Queries;

public class GetClientByIdQuery : IGetClientByIdQuery
{
    private readonly IClientsRepository _clientsRepository;

    public GetClientByIdQuery(IClientsRepository clientsRepository)
    {
        _clientsRepository = clientsRepository;
    }

    public async Task<ClientDto?> GetByIdAsync(Guid id)
    {
        var client = await _clientsRepository.GetByIdAsync(id);

        return client is null
            ? null
            : new ClientDto(client.Id, client.Name, client.Email, client.BalanceT,
                client.Tags.Select(t => new TagDto(t.Name, t.Color)).ToList());
    }
}