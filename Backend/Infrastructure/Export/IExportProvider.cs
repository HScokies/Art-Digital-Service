using Contracts.File;
using Contracts.Participant;

namespace Infrastructure.Export
{
    public interface IExportProvider
    {
         FileResponse ExportParticipants(ParticipantExportModel[] participants);
    }
}
