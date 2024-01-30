namespace Domain.Repositories
{
    public interface IRepository
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<DTO[]> GetAsync<DTO>(CancellationToken cancellationToken) where DTO : class;
        Task<DTO> CreateAsync<DTO>(DTO entity, CancellationToken cancellationToken) where DTO : class;
    }
}
