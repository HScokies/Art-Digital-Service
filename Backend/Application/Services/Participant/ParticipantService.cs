using Contracts.File;
using Domain.Core.Primitives;
using Infrastructure.Files;

namespace Application.Services.Participant
{
    internal class ParticipantService : IParticipantService
    {
        private readonly IFilesService filesService;

        public ParticipantService(IFilesService filesService)
        {
            this.filesService = filesService;
        }

        public Task<Result<FileResponse>> GetParticipantFile(string filename, CancellationToken cancellationToken) => filesService.DownloadUserFileAsync(filename, cancellationToken);
    }
}
