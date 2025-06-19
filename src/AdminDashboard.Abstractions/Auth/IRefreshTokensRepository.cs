using AdminDashboard.Abstractions.Common;
using AdminDashboard.Domain;

namespace AdminDashboard.Abstractions.Auth;

public interface IRefreshTokensRepository : IRepository<RefreshToken, Guid>
{
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task RevokeTokenAsync(string token);
    Task<bool> IsTokenActiveAsync(string token);
}