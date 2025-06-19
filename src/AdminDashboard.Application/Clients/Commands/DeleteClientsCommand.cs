using AdminDashboard.Abstractions.Clients;
using AdminDashboard.Abstractions.Clients.Commands;
using AdminDashboard.Contracts.Clients;

namespace AdminDashboard.Application.Clients.Commands;

public class DeleteClientsCommand : IDeleteClientsCommand
{
    private readonly IClientsRepository _clientsRepository;

    public DeleteClientsCommand(IClientsRepository clientsRepository)
    {
        _clientsRepository = clientsRepository;
    }

    public async Task ExecuteAsync(DeleteClientsRequest request, CancellationToken cancellationToken)
    {
        if (request.ClientIds.Length == 0)
        {
            return;
        }

        await _clientsRepository.DeleteMultipleAsync(request.ClientIds);
    }
}