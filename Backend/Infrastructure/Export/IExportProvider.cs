using Contracts.File;
using Contracts.Participant;
using Contracts.Staff;

namespace Infrastructure.Export
{
    public interface IExportProvider
    {
        FileResponse ExportParticipants(ParticipantExportModel[] participants);
        FileResponse ExportStaff(StaffExportModel[] staff);       
    }
}
