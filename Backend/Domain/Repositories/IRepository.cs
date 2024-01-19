namespace Domain.Repositories
{
    public interface IRepository
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
