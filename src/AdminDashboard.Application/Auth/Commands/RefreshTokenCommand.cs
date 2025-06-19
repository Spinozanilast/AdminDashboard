using System.Security.Claims;
using AdminDashboard.Abstractions;
using AdminDashboard.Abstractions.Auth;
using AdminDashboard.Abstractions.Auth.Commands;
using AdminDashboard.Contracts.Auth;

namespace AdminDashboard.Application.Auth.Commands;

public class RefreshTokenCommand : IRefreshTokenCommand
{
    private readonly IUsersRepository _usersRepository;
    private readonly IJwtService _jwtService;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenCommand(IUsersRepository usersRepository, IJwtService jwtService, IUnitOfWork unitOfWork)
    {
        _usersRepository = usersRepository;
        _jwtService = jwtService;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponse> ExecuteAsync(RefreshTokenRequest request,
        CancellationToken cancellationToken = default)
    {
        var principal = _jwtService.GetPrincipalFromExpiredToken(request.Token);

        if (principal is null)
        {
            throw new Exception("Invalid or empty token");
        }

        var userId = Guid.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        var user = await _usersRepository.GetByIdAsync(userId);
        if (user is null)
        {
            throw new UnauthorizedAccessException("User not found");
        }

        var storedRefreshToken = user.RefreshTokens.FirstOrDefault(rt =>
            rt.Token == request.RefreshToken);

        if (storedRefreshToken is null || !storedRefreshToken.IsActive)
        {
            throw new UnauthorizedAccessException("Invalid refresh token");
        }

        storedRefreshToken.Revoke();

        var newToken = _jwtService.GenerateAccessToken(user);
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        user.AddRefreshToken(newRefreshToken);
        await _usersRepository.UpdateAsync(user);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new AuthResponse(newToken, newRefreshToken.Token);
    }
}