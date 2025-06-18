using AdminDashboard.Domain.Common;

namespace AdminDashboard.Domain;

public class Client : Entity<Guid>
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public decimal BalanceT { get; private set; }
    public List<Payment> Payments { get; } = new();
    public List<ClientTag> Tags { get; } = new();

    private Client()
    {
    }

    public Client(string name, string email, decimal balanceT)
    {
        Name = name;
        Email = email;
        BalanceT = balanceT;
    }

    public void Update(string name, string email, decimal balanceT)
    {
        Name = name;
        Email = email;
        BalanceT = balanceT;
    }

    public void AddPayment(Payment payment)
    {
        Payments.Add(payment);
        BalanceT += payment.Amount;
    }

    public void AddTag(Tag tag)
    {
        if (Tags.All(t => t.TagId != tag.Id))
        {
            Tags.Add(new ClientTag(Id, tag.Id));
        }
    }

    public void RemoveTag(Guid tagId)
    {
        var tagToRemove = Tags.FirstOrDefault(t => t.TagId == tagId);
        if (tagToRemove != null)
        {
            Tags.Remove(tagToRemove);
        }
    }
}