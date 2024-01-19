using Domain.Repositories;

namespace Data.Repositories
{
    internal class Repository : IRepository
    {
        private readonly AppDbContext ctx;

        public Repository(AppDbContext ctx) => this.ctx = ctx;
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken) => await ctx.SaveChangesAsync(cancellationToken);
    }
}
