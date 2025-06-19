using System.Security.Claims;
using AdminDashboard.Domain;

namespace AdminDashboard.Abstractions.Auth;

public interface IJwtService
{
    string GenerateAccessToken(User user);
    RefreshToken GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}