using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<bool> ExistsAsync(string email, CancellationToken cancellationToken);
        Task<UserDto?> GetUserByEmailAsync (string email, CancellationToken cancellationToken);
        Task<UserDto?> GetUserByIdAsync (int id, CancellationToken cancellationToken);
        Task<ParticipantDto?> GetParticipantByUserIdAsync(int id, CancellationToken cancellationToken);
        IEnumerable<UserDto> GetUsersByParticipantIds(int[] id);
        Task DropAsync(IEnumerable<UserDto> users, CancellationToken cancellationToken);
    }
}
