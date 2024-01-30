using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    internal class Repository : IRepository
    {
        private readonly AppDbContext ctx;

        public Repository(AppDbContext ctx) => this.ctx = ctx;

        public async Task<DTO> CreateAsync<DTO>(DTO entity, CancellationToken cancellationToken) where DTO : class
        {
            var res = await ctx.Set<DTO>().AddAsync(entity, cancellationToken);
            await ctx.SaveChangesAsync(cancellationToken);
            return res.Entity;
        }

        public async Task<DTO[]> GetAsync<DTO>(CancellationToken cancellationToken) where DTO : class => await ctx.Set<DTO>().ToArrayAsync(cancellationToken);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken) => await ctx.SaveChangesAsync(cancellationToken);
    }
}
