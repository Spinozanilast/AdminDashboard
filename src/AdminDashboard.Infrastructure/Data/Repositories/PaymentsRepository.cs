using AdminDashboard.Abstractions.Payments;
using AdminDashboard.Domain;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Infrastructure.Data.Repositories;

public class PaymentsRepository(AppDbContext context) : IPaymentsRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Payment?> GetByIdAsync(Guid id)
    {
        return await _context.Payments.FindAsync(id);
    }

    public async Task<IEnumerable<Payment>> GetAllAsync()
    {
        return await _context.Payments.ToListAsync();
    }

    public async Task AddAsync(Payment payment)
    {
        var client = await _context.Clients.FindAsync(payment.ClientId);

        if (client is not null)
        {
            client.AddPayment(payment);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(Payment payment)
    {
        _context.Payments.Update(payment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var payment = await GetByIdAsync(id);
        if (payment is not null)
        {
            //TODO: Extra logic need to be here
            // var client = await _context.Clients.FindAsync(payment.ClientId);

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Payment>> GetLatestPaymentsAsync(int count = 5)
    {
        return await _context.Payments
            .OrderByDescending(p => p.Date)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetPaymentsByClientIdAsync(Guid clientId)
    {
        return await _context.Payments
            .Where(p => p.ClientId == clientId)
            .OrderByDescending(p => p.Date)
            .ToListAsync();
    }
}