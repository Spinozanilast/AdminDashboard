using AdminDashboard.Contracts.Auth;

namespace AdminDashboard.Abstractions.Auth.Commands;

public interface IRefreshTokenCommand
{
    Task<AuthResponse> ExecuteAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
}