using Data.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext ctx;

        public RefreshTokenRepository(AppDbContext ctx) => this.ctx = ctx;

        public async Task CreateAsync(TokenDto entity, CancellationToken cancellationToken)
        {
            await ctx.tokens.AddAsync(entity, cancellationToken);
            await ctx.SaveChangesAsync(cancellationToken);
        }

        public async Task DropAsync(TokenDto token, CancellationToken cancellationToken)
        {
            ctx.tokens.Remove(token); 
            await ctx.SaveChangesAsync(cancellationToken);
        }

        public async Task DropAsync(string deviceId, CancellationToken cancellationToken)
        {
            var entity = await ctx.tokens.FirstOrDefaultAsync(t => t.deviceId == deviceId, cancellationToken);
            if (entity is null) return;
            await DropAsync(entity, cancellationToken);
        }

        public async Task DropExpiredAsync(CancellationToken cancellationToken)
        {
            var expired = ctx.tokens.Where(t => t.expires < DateTime.UtcNow);
            await expired.ExecuteDeleteAsync(cancellationToken);
            await ctx.SaveChangesAsync(cancellationToken);
        }

        public async Task<TokenDto?> GetAsync(string deviceId, int userId, CancellationToken cancellationToken) => await ctx.tokens.FirstOrDefaultAsync(t => t.deviceId == deviceId && t.userId == userId, cancellationToken);
    }
}
