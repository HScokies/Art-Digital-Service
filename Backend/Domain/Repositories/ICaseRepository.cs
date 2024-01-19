using Domain.Entities;

namespace Domain.Repositories
{
    public interface ICaseRepository
    {
        public Task<bool> ExistsAsync(int caseId, CancellationToken cancellationToken);
        public Task<CaseDto[]> List(CancellationToken cancellationToken);
        public Task<CaseDto?> GetCase(int caseId, CancellationToken cancellationToken);
    }
}
