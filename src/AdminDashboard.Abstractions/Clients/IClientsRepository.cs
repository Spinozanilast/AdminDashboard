using AdminDashboard.Abstractions.Common;
using AdminDashboard.Domain;

namespace AdminDashboard.Abstractions.Clients;

public interface IClientsRepository: IRepository<Client, Guid>
{
    Task DeleteMultipleAsync(IEnumerable<Guid> ids);
}