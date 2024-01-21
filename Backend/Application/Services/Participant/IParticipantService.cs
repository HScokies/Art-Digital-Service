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
        Task<Result<int>> CreateParticipantAsync(CreateParticipantRequest request, CancellationToken cancellationToken);
        Task<ParticipantTypeDto[]> GetParticipantTypesAsync(CancellationToken cancellationToken);
        Task<GetParticipantResponse> GetParticipantsAsync(CancellationToken cancellationToken, int offset, int take, bool participantsOnly, bool hasScore = true, bool noScore = true, string? search = null, List<int>? excludeType = null, List<int>? excludeCase = null);
        Task<Result<ParticipantDto>> GetParticipantAsync(int participantId, CancellationToken cancellationToken);
        Task<Result<bool>> UpdateParticipantAsync(int participantId, UpdateParticipantRequest request, CancellationToken cancellationToken);
        Task<Result<bool>> RateParticipantAsync(int participantId, RateParticipantRequest request, CancellationToken cancellationToken);
        Task<Result<FileResponse>> GetParticipantFileAsync(string filename, CancellationToken cancellationToken);
        Task<Result<bool>> DropParticipantAsync(int participantId, CancellationToken cancellationToken);
        FileResponse ExportParticipants(int[]? participants);
    }
}
