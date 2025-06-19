using AdminDashboard.Abstractions.Clients;
using AdminDashboard.Abstractions.Clients.Queries;
using AdminDashboard.Contracts.Clients;
using AdminDashboard.Contracts.Tags;

namespace AdminDashboard.Application.Clients.Queries;

public class GetClientsQuery : IGetClientsQuery
{
    private readonly IClientsRepository _clientsRepository;

    public GetClientsQuery(IClientsRepository clientsRepository)
    {
        _clientsRepository = clientsRepository;
    }

    public async Task<List<ClientDto>> GetClientsAsync(CancellationToken cancellationToken)
    {
        var clients = await _clientsRepository.GetAllAsync();

        return clients.Select(client => new ClientDto
        (
            client.Id,
            client.Name,
            client.Email,
            client.BalanceT,
            client.Tags.Select(t => new TagDto(t.Name, t.Color)).ToList()
        )).ToList();
    }
}