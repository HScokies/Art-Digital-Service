using Domain.Entities;

namespace Domain.Repositories
{
    public interface ICaseRepository
    {
        public Task<bool> ExistsAsync(int caseId);
    }
}
