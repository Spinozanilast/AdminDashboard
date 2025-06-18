using AdminDashboard.Domain.Common;

namespace AdminDashboard.Domain;

public class RefreshToken : Entity<Guid>
{
    public string Token { get; private set; }
    public DateTime Created { get; private set; }
    public DateTime Expires { get; private set; }
    public DateTime? Revoked { get; private set; }
    public Guid UserId { get; private set; }

    private RefreshToken()
    {
    }

    public RefreshToken(string token, DateTime expires, Guid userId)
    {
        Token = token;
        Created = DateTime.UtcNow;
        Expires = expires;
        UserId = userId;
    }

    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsActive => !IsExpired && Revoked == null;

    public void Revoke()
    {
        Revoked = DateTime.UtcNow;
    }
}