using AdminDashboard.Domain.Common;

namespace AdminDashboard.Domain;

public class Payment : Entity<Guid>
{
    public decimal Amount { get; private set; }
    public DateTime Date { get; private set; }
    public string Description { get; private set; }
    public Guid ClientId { get; private set; }

    private Payment()
    {
    }

    public Payment(decimal amount, string description, Guid clientId)
    {
        Amount = amount;
        Description = description;
        ClientId = clientId;
        Id = Guid.NewGuid();
        Date = DateTime.UtcNow;
    }
}