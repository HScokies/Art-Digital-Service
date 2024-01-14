using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<bool> CheckIfUserExistsAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> CheckIfTypeExistsAsync(int participantTypeId, CancellationToken cancellationToken = default);
        Task<ParticipantDto> CreateParticipantAsync(ParticipantDto participant, CancellationToken cancellationToken = default);
        Task<UserDto?> GetUserByEmail (string email, CancellationToken cancellationToken = default);
    }
}
