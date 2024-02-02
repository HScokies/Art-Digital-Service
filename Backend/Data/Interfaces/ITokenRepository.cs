using Domain.Entities;

namespace Data.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<TokenDto?> GetAsync(string deviceId, int userId, CancellationToken cancellationToken);
        Task CreateAsync(TokenDto entity, CancellationToken cancellationToken);
        Task DropAsync(TokenDto token, CancellationToken cancellationToken);
        Task DropAsync(string deviceId, CancellationToken cancellationToken);
        Task DropExpiredAsync(CancellationToken cancellationToken);
    }
}
