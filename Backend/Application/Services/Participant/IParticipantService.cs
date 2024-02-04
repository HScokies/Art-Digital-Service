using Contracts.File;
using Contracts.Participant;
using Contracts.User;
using Domain.Core.Primitives;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Participant
{
    public interface IParticipantService
    {
        Task<Result<PersonalDataAppendResponse>> AppendParticipantDataAsync(int userId, PersonalDataAppendRequest request, CancellationToken cancellationToken);
        Task<Result<AppendParticipantFilesResponse>> AppendParticipantFilesAsync(int userId, IFormFile consent, IFormFile solution, CancellationToken cancellationToken);
        Task<Result<int>> CreateAsync(CreateParticipantRequest request, CancellationToken cancellationToken);
        Task<ParticipantTypeDto[]> GetParticipantTypesAsync(CancellationToken cancellationToken);
        Task<GetParticipantsResponse> GetParticipantsAsync(CancellationToken cancellationToken, int offset, int take, bool participantsOnly, bool asc = true, string? orderBy = null, bool hasScore = true, bool noScore = true, string? search = null, int[]? excludeType = null, int[]? excludeCase = null);
        Task<Result<GetParticipantResponse>> GetParticipantAsync(int participantId, CancellationToken cancellationToken);
        Task<Result<bool>> UpdateParticipantAsync(int participantId, UpdateParticipantRequest request, CancellationToken cancellationToken);
        Task<Result<bool>> RateParticipantAsync(int participantId, RateParticipantRequest request, CancellationToken cancellationToken);
        Task<Result<FileResponse>> GetParticipantFileAsync(string filename, CancellationToken cancellationToken);
        Task DropParticipantsAsync(int[] participantIds, CancellationToken cancellationToken);
        Task<FileResponse> ExportParticipantsAsync(int[]? participants, CancellationToken cancellationToken);
        Task<bool> isAdultAsync(int userId, CancellationToken cancellationToken);
        Task<Result<GetProfileResponse>> GetProfileAsync(int userId, CancellationToken cancellationToken);
    }
}
