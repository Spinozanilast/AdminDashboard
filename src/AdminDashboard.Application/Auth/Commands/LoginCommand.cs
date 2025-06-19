using AdminDashboard.Abstractions;
using AdminDashboard.Abstractions.Auth;
using AdminDashboard.Abstractions.Auth.Commands;
using AdminDashboard.Contracts.Auth;

namespace AdminDashboard.Application.Auth.Commands;

public class LoginCommand : ILoginCommand
{
    private readonly IUsersRepository _usersRepository;
    private readonly IJwtService _jwtService;
    private readonly IUnitOfWork _unitOfWork;

    public LoginCommand(IUsersRepository usersRepository, IJwtService jwtService, IUnitOfWork unitOfWork)
    {
        _usersRepository = usersRepository;
        _jwtService = jwtService;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponse> ExecuteAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _usersRepository.GetByEmailAsync(request.Email);
        
        if (user is null || !user.VerifyPassword(request.Password))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var token = _jwtService.GenerateAccessToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();

        user.AddRefreshToken(refreshToken);
        await _usersRepository.UpdateAsync(user);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new AuthResponse(token, refreshToken.Token);
    }
}