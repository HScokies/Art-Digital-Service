using Contracts.File;
using Domain.Core.Primitives;

namespace Application.Services.Participant
{
    public interface IParticipantService
    {
        public Task<Result<FileResponse>> GetParticipantFile(string filename, CancellationToken cancellationToken);
    }
}
