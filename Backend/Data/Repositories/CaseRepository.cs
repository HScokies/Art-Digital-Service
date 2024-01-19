using Domain.Entities;
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
        public async Task<bool> ExistsAsync(int caseId, CancellationToken cancellationToken) => await ctx.cases.AnyAsync(c => c.id == caseId, cancellationToken);

        public async Task<CaseDto?> GetCase(int caseId, CancellationToken cancellationToken) => await ctx.cases.FirstOrDefaultAsync(c => c.id == caseId, cancellationToken);

        public async Task<CaseDto[]> List(CancellationToken cancellationToken)  => await ctx.cases.ToArrayAsync(cancellationToken);
    }
}
