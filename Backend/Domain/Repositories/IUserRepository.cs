using Domain.Core.Primitives;
using Contracts.User;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<bool> CheckIfExistsAsync(string email, CancellationToken cancellationToken = default);
        Task<ParticipantTypeDto?> GetParticipantTypeAsync(int participantTypeId, CancellationToken cancellationToken = default);
        Task<UserDto> CreateUserAsync(string email, string password, CancellationToken cancellationToken = default);
        Task<ParticipantDto> CreateParticipantAsync(UserDto user, ParticipantTypeDto type, CancellationToken cancellationToken = default);
    }
}
