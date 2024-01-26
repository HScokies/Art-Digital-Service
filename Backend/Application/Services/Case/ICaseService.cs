using Contracts.Cases;
using Domain.Core.Primitives;
using Domain.Entities;

namespace Application.Services.Case
{
    public interface ICaseService
    {
        Task<int> CreateAsync(UpsertCaseRequest request,CancellationToken cancellationToken);
        IEnumerable<CasePreviewResponse> Get(string? search);
        Task<Result<CaseDto>> GetAsync(int id, CancellationToken cancellationToken);
        Task<Result<bool>> UpdateAsync(int id, UpsertCaseRequest request, CancellationToken cancellationToken);
        Task<Result<bool>> DropAsync(int id, CancellationToken cancellationToken);
    }
}
