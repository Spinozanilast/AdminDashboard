using AdminDashboard.Abstractions.Clients;
using AdminDashboard.Domain;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Infrastructure.Data.Repositories;

public class ClientsRepository(AppDbContext context) : IClientsRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Client?> GetByIdAsync(Guid id)
    {
        return await _context.Clients
            .AsNoTracking()
            .Include(c => c.Payments)
            .Include(c => c.Tags)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await _context.Clients
            .AsNoTracking()
            .Include(c => c.Tags)
            .ToListAsync();
    }

    public async Task AddAsync(Client client)
    {
        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Client client)
    {
        _context.Clients.Update(client);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var client = await GetByIdAsync(id);

        if (client is not null)
        {
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteMultipleAsync(IEnumerable<Guid> ids)
    {
        await _context.Clients.Where(c => ids.Contains(c.Id)).ExecuteDeleteAsync();
        await _context.SaveChangesAsync();
    }
}