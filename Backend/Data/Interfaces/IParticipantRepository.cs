using Contracts.Participant;
using Contracts.User;
using Domain.Entities;

namespace Data.Interfaces
{
    public interface IParticipantRepository
    {
        Task<GetParticipantsResponse> GetAsync(CancellationToken cancellationToken, int offset, int take, bool asc = true, string? orderBy = null, bool participantsOnly = false, bool hasScore = true, bool noScore = true, string? search = null, int[]? excludeType = null, int[]? excludeCase = null);
        Task<ParticipantDto[]> GetAsync(int[]? id, CancellationToken cancellationToken);
        Task<ParticipantExportModel[]> GetExportModelsAsync(int[]? id, CancellationToken cancellationToken);
        Task<ParticipantDto?> GetAsync(int id, CancellationToken cancellationToken);
        Task<bool> TypeExistsAsync(int participantTypeId, CancellationToken cancellationToken);
        Task<ParticipantDto> CreateAsync(ParticipantDto participant, CancellationToken cancellationToken);
        Task<ParticipantTypeDto[]> GetTypesAsync(CancellationToken cancellationToken);
        Task<bool> isAdult(int userId, CancellationToken cancellationToken);
        Task<GetProfileResponse?> GetProfileAsync(int userId, CancellationToken cancellationToken);
        Task DropAsync(ParticipantDto[] participants, CancellationToken cancellationToken);

        Task<GetCertificateRequest?> GetCertificateContentAsync(int userId, CancellationToken cancellationToken);
    }
}
