namespace AdminDashboard.Contracts.Auth;

public record RefreshTokenRequest(string AccessToken, string RefreshToken);