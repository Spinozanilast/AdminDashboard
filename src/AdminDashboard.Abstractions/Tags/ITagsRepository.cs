using AdminDashboard.Abstractions.Common;
using AdminDashboard.Domain;

namespace AdminDashboard.Abstractions.Tags;

public interface ITagsRepository: IRepository<Tag, Guid>
{
    Task<Tag?> GetByNameAsync(string name);
    Task<IEnumerable<Tag>> GetTagsForClientAsync(Guid clientId);
}