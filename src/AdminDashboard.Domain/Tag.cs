using AdminDashboard.Domain.Common;

namespace AdminDashboard.Domain;

public class Tag: Entity<Guid>
{
    public string Name { get; private set; }
    public string Color { get; private set; }
    public List<ClientTag> Clients { get; } = new();

    private Tag() {}

    public Tag(string name, string color = "#4f46e5")
    {
        Name = name;
        Color = color;
    }

    public void Update(string name, string color)
    {
        Name = name;
        Color = color;
    }
}