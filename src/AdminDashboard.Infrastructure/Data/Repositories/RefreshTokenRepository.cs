using AdminDashboard.Abstractions.Auth;
using AdminDashboard.Domain;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboard.Infrastructure.Data.Repositories;

public class RefreshTokenRepository(AppDbContext context) : IRefreshTokensRepository
{
    private readonly AppDbContext _context = context;

    public async Task<RefreshToken?> GetByIdAsync(Guid id)
    {
        return await _context.RefreshTokens.FindAsync(id);
    }

    public async Task<IEnumerable<RefreshToken>> GetAllAsync()
    {
        return await _context.RefreshTokens.ToListAsync();
    }

    public async Task AddAsync(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Update(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var token = await GetByIdAsync(id);
        if (token is not null)
        {
            _context.RefreshTokens.Remove(token);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token);
    }

    public async Task RevokeTokenAsync(string token)
    {
        var refreshToken = await GetByTokenAsync(token);
        if (refreshToken != null)
        {
            refreshToken.Revoke();
            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> IsTokenActiveAsync(string token)
    {
        var refreshToken = await GetByTokenAsync(token);
        return refreshToken is not null && refreshToken.IsActive;
    }
}