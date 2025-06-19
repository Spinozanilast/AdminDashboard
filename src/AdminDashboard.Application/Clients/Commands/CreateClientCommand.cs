using AdminDashboard.Abstractions;
using AdminDashboard.Abstractions.Clients;
using AdminDashboard.Abstractions.Clients.Commands;
using AdminDashboard.Abstractions.Tags;
using AdminDashboard.Contracts.Clients;
using AdminDashboard.Domain;

namespace AdminDashboard.Application.Clients.Commands;

public class CreateClientCommand : ICreateClientCommand
{
    private readonly IClientsRepository _clientRepository;
    private readonly ITagsRepository _tagRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateClientCommand(
        IClientsRepository clientRepository,
        ITagsRepository tagRepository,
        IUnitOfWork unitOfWork)
    {
        _clientRepository = clientRepository;
        _tagRepository = tagRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> ExecuteAsync(CreateClientRequest request, CancellationToken cancellationToken = default)
    {
        var client = new Client(request.Name, request.Email, request.BalanceT);

        if (request.Tags.Count != 0)
        {
            foreach (var tagName in request.Tags)
            {
                var tag = await _tagRepository.GetByNameAsync(tagName)
                          ?? new Tag(tagName);

                client.AddTag(tag);
            }
        }

        await _clientRepository.AddAsync(client);
        await _unitOfWork.CommitAsync(cancellationToken);

        return client.Id;
    }
}