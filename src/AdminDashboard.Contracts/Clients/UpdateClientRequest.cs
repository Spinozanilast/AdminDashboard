namespace AdminDashboard.Contracts.Clients;

public class UpdateClientRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public decimal BalanceT { get; set; }
    public List<string> Tags { get; set; }
}