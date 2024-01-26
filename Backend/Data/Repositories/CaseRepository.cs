using Contracts.Cases;
using Data.Interfaces;
using Domain.Entities;
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

        public IEnumerable<CasePreviewResponse> Get(string? search = null)
        {
            var response = ctx.cases.AsQueryable();
            if (search is not null)
                response = response.Where(c => c.name.ToLower().Contains(search));

            return response.Select(c => new CasePreviewResponse()
            {
                id = c.id,
                name = c.name
            });
        }

        public async Task<CaseDto?> GetAsync(int caseId, CancellationToken cancellationToken) => await ctx.cases.FirstOrDefaultAsync(c => c.id == caseId, cancellationToken);
        public async Task<CaseDto> CreateAsync(CaseDto entity, CancellationToken cancellationToken)
        {
            var res = await ctx.cases.AddAsync(entity, cancellationToken);
            await ctx.SaveChangesAsync(cancellationToken);
            return res.Entity;
        }

        public async Task DropAsync(CaseDto entity, CancellationToken cancellationToken)
        {
            ctx.cases.Remove(entity);
            await ctx.SaveChangesAsync(cancellationToken);
        }
    }
}
