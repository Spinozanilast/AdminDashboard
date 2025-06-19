namespace AdminDashboard.Contracts.Clients;

public record CreateClientRequest(string Name, string Email, decimal BalanceT, List<string> Tags);  