using AdminDashboard.Abstractions.Tags;
using AdminDashboard.Domain;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Infrastructure.Data.Repositories;

public class TagsRepository(AppDbContext context) : ITagsRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Tag?> GetByIdAsync(Guid id)
    {
        return await _context.Tags
            .Include(t => t.Clients)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        return await _context.Tags.ToListAsync();
    }

    public async Task AddAsync(Tag tag)
    {
        await _context.Tags.AddAsync(tag);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Tag tag)
    {
        _context.Tags.Update(tag);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var tag = await GetByIdAsync(id);
        if (tag is not null)
        {
            foreach (var client in tag.Clients.ToList())
            {
                client.RemoveTag(tag.Id);
            }

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Tag?> GetByNameAsync(string name)
    {
        return await _context.Tags
            .FirstOrDefaultAsync(t => t.Name == name);
    }

    public async Task<IEnumerable<Tag>> GetTagsForClientAsync(Guid clientId)
    {
        return await _context.Tags
            .Where(t => t.Clients.Any(c => c.Id == clientId))
            .ToListAsync();
    }
}