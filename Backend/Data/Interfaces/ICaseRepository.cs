using Contracts.Cases;
using Domain.Entities;

namespace Data.Interfaces
{
    public interface ICaseRepository
    {
        Task<bool> ExistsAsync(int caseId, CancellationToken cancellationToken);
        IEnumerable<CasePreviewResponse> Get(string? search);
        Task<CaseDto?> GetAsync(int caseId, CancellationToken cancellationToken);
        Task<CaseDto> CreateAsync(CaseDto entity, CancellationToken cancellationToken);
        Task DropAsync(CaseDto entity, CancellationToken cancellationToken);
    }
}
