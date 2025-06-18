namespace AdminDashboard.Domain.Common;

public class Entity<T> where T : struct
{
    public T Id { get; init; }
}