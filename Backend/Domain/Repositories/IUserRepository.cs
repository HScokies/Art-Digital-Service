using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<bool> UserExistsAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> TypeExistsAsync(int participantTypeId, CancellationToken cancellationToken = default);
        Task<ParticipantDto> CreateParticipantAsync(ParticipantDto participant, CancellationToken cancellationToken = default);
        Task<UserDto?> GetUserByEmailAsync (string email, CancellationToken cancellationToken = default);
        Task<UserDto?> GetUserByIdAsync (int id, CancellationToken cancellationToken = default);
        Task<ParticipantDto?> GetParticipantByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ParticipantTypeDto[]> GetParticipantTypes(CancellationToken cancellationToken = default);
    }
}
