using AdminDashboard.Domain.Common;

namespace AdminDashboard.Abstractions.Common;

public interface IRepository<TEntity, in TKey> where TEntity : Entity<TKey> where TKey : struct
{
    Task<TEntity?> GetByIdAsync(TKey id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task AddAsync(TEntity rate);
    Task UpdateAsync(TEntity rate);
    Task DeleteAsync(TKey id);
}