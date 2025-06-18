using AdminDashboard.Domain.Common;

namespace AdminDashboard.Domain;

public class ClientTag: Entity<Guid>
{
    public Guid ClientId { get; private set; }
    public Guid TagId { get; private set; }

    private ClientTag() {}

    public ClientTag(Guid clientId, Guid tagId)
    {
        ClientId = clientId;
        TagId = tagId;
    }
}