using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<bool> UserExistsAsync(string email, CancellationToken cancellationToken);
        Task<UserDto?> GetUserByEmailAsync (string email, CancellationToken cancellationToken);
        Task<UserDto?> GetUserByIdAsync (int id, CancellationToken cancellationToken);
        Task<ParticipantDto?> GetParticipantByUserIdAsync(int id, CancellationToken cancellationToken);
    }
}
