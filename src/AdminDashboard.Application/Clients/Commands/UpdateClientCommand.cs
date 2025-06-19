using System.Collections.Immutable;
using AdminDashboard.Abstractions;
using AdminDashboard.Abstractions.Clients;
using AdminDashboard.Abstractions.Clients.Commands;
using AdminDashboard.Abstractions.Tags;
using AdminDashboard.Contracts.Clients;
using AdminDashboard.Domain;
using AdminDashboard.Domain.Exceptions;

namespace AdminDashboard.Application.Clients.Commands;

public class UpdateClientCommand : IUpdateClientCommand
{
    private readonly IClientsRepository _clientRepository;
    private readonly ITagsRepository _tagRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateClientCommand(IClientsRepository clientRepository, ITagsRepository tagRepository,
        IUnitOfWork unitOfWork)
    {
        _clientRepository = clientRepository;
        _tagRepository = tagRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsync(UpdateClientRequest request, CancellationToken cancellationToken = default)
    {
        var client = await _clientRepository.GetByIdAsync(request.Id);
        if (client is null)
        {
            throw new NotFoundException(nameof(Client), request.Id);
        }

        client.Update(request.Name, request.Email, request.BalanceT);

        var currentTagsNames = client.Tags.Select(t => t.Name).ToList();
        var tagsToAdd = request.Tags.Except(currentTagsNames).ToList();
        var tagsToRemove = currentTagsNames.Except(request.Tags).ToList();

        foreach (var tagName in tagsToAdd)
        {
            var tag = await _tagRepository.GetByNameAsync(tagName);

            if (tag is null)
            {
                //TODO: change update client request to get tag dto with color
                tag = new Tag(tagName);
                await _tagRepository.AddAsync(tag);
            }

            client.AddTag(tag);
        }

        foreach (var tagName in tagsToRemove)
        {
            var tag = client.Tags.FirstOrDefault(t => t.Name == tagName);
            if (tag is not null) client.RemoveTag(tag.Id);
        }

        await _clientRepository.UpdateAsync(client);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}