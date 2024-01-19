using Contracts.User;

namespace Data.Interfaces
{
    public interface IParticipantRepository
    {
        public Task<GetParticipantResponse> GetParticipants(CancellationToken cancellationToken, int offset, int take, bool participantsOnly = false, bool hasScore = true, bool noScore = true, string? search = null, List<int>? excludeType = null, List<int>? excludeCase = null);
    }
}
