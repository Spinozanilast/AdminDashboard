using AdminDashboard.Contracts.Auth;

namespace AdminDashboard.Abstractions.Auth.Commands;

public interface ILoginCommand
{
    Task<AuthResponse> ExecuteAsync(LoginRequest request, CancellationToken cancellationToken = default);
}