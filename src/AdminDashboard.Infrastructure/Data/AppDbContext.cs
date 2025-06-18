using AdminDashboard.Domain;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<ExchangeRate> ExchangeRates => Set<ExchangeRate>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}