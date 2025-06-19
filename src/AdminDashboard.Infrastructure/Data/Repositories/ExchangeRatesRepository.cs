using AdminDashboard.Abstractions.ExchangeRates;
using AdminDashboard.Domain;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Infrastructure.Data.Repositories;

public class ExchangeRatesRepository(AppDbContext context) : IExchangeRatesRepository
{
    private readonly AppDbContext _context = context;

    public async Task<ExchangeRate?> GetByIdAsync(Guid id)
    {
        return await _context.ExchangeRates.FindAsync(id);
    }

    public async Task<IEnumerable<ExchangeRate>> GetAllAsync()
    {
        return await _context.ExchangeRates.ToListAsync();
    }

    public async Task AddAsync(ExchangeRate rate)
    {
        await _context.ExchangeRates.AddAsync(rate);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ExchangeRate rate)
    {
        _context.ExchangeRates.Update(rate);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var rate = await GetByIdAsync(id);
        if (rate is not null)
        {
            _context.ExchangeRates.Remove(rate);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<ExchangeRate?> GetCurrentRateAsync()
    {
        return await _context.ExchangeRates
            .OrderByDescending(e => e.LastUpdated)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateRateAsync(decimal newRate)
    {
        var currentRate = new ExchangeRate(newRate);
        await AddAsync(currentRate);
        await _context.SaveChangesAsync();
    }
}