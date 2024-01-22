using Contracts.Participant;
using Contracts.User;
using Domain.Entities;

namespace Data.Interfaces
{
    public interface IParticipantRepository
    {
        Task<GetParticipantResponse> GetParticipantsAsync(CancellationToken cancellationToken, int offset, int take, bool participantsOnly = false, bool hasScore = true, bool noScore = true, string? search = null, List<int>? excludeType = null, List<int>? excludeCase = null);
        Task<ParticipantDto[]> GetParticipantsAsync(int[]? id, CancellationToken cancellationToken);
        Task<ParticipantExportModel[]> GetParticipantExportModelsAsync(int[]? id, CancellationToken cancellationToken);
        Task<ParticipantDto?> GetParticipantByIdAsync(int id, CancellationToken cancellationToken);
        Task<bool> TypeExistsAsync(int participantTypeId, CancellationToken cancellationToken);
        Task<ParticipantDto> CreateParticipantAsync(ParticipantDto participant, CancellationToken cancellationToken);
        Task<ParticipantTypeDto[]> GetParticipantTypesAsync(CancellationToken cancellationToken);
        Task DropParticipantAsync(ParticipantDto participant, CancellationToken cancellationToken);
        Task DropParticipantsAsync(ParticipantDto[] participants, CancellationToken cancellationToken);
    }
}
