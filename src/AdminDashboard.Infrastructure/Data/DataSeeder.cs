using AdminDashboard.Domain;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Infrastructure.Data;

public static class DataSeeder
{
    public static void SeedContextData(this ModelBuilder modelBuilder)
    {
        // Exchange by default
        modelBuilder.Entity<ExchangeRate>().HasData(new ExchangeRate(10.0m));

        // User
        var userId = Guid.NewGuid();
        modelBuilder.Entity<User>().HasData(
            new User("admin@mirra.dev", BCrypt.Net.BCrypt.HashPassword("admin123"))
                { Id = userId }
        );

        // Tags
        var tag1 = new Tag("VIP", "#ff0000");
        var tag2 = new Tag("Regular", "#00ff00");

        modelBuilder.Entity<Tag>().HasData(tag1, tag2);

        // Clients
        var client1 = new Client("John Doe", "john@example.com", 100);
        var client2 = new Client("Jane Smith", "jane@example.com", 200);
        var client3 = new Client("Bob Johnson", "bob@example.com", 300);

        modelBuilder.Entity<Client>().HasData(client1, client2, client3);

        modelBuilder.Entity("ClientTag").HasData(
            new { ClientsId = client1.Id, TagsId = tag1.Id },
            new { ClientsId = client2.Id, TagsId = tag1.Id },
            new { ClientsId = client3.Id, TagsId = tag2.Id }
        );

        // Payments
        modelBuilder.Entity<Payment>().HasData(
            new Payment(50, "Payment 1", client1.Id),
            new Payment(75, "Payment 2", client1.Id),
            new Payment(100, "Payment 3", client2.Id),
            new Payment(25, "Payment 4", client3.Id),
            new Payment(150, "Payment 5", client3.Id)
        );
    }
}