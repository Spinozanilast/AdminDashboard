using AdminDashboard.Domain.Common;

namespace AdminDashboard.Domain;

public class User : Entity<Guid>
{
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public List<RefreshToken> RefreshTokens { get; } = new();
    
    private User() {}

    public User(string email, string password)
    {
        Email = email;
        SetPassword(password);
    }

    public void SetPassword(string password)
    {
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
    }

    public void AddRefreshToken(RefreshToken token)
    {
        RefreshTokens.Add(token);
    }

    public void RevokeRefreshToken(string token)
    {
        var refreshToken = RefreshTokens.FirstOrDefault(rt => rt.Token == token);
        if (refreshToken != null)
        {
            refreshToken.Revoke();
        }
    }
}