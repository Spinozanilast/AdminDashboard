namespace AdminDashboard.Contracts.Auth;

public record AuthResponse(string JwtToken, string RefreshToken);