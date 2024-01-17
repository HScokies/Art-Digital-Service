using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class CaseRepository : ICaseRepository
    {
        private readonly AppDbContext ctx;

        public CaseRepository(AppDbContext ctx)
        {
            this.ctx = ctx;
        }
        public async Task<bool> ExistsAsync(int caseId) => await ctx.cases.AnyAsync(c => c.id == caseId);
    }
}
