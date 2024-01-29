using Contracts.Staff;
using Domain.Entities;

namespace Data.Interfaces
{
    public interface IStaffRepository
    {
        Task<StaffDto> CreateAsync(StaffDto staff, CancellationToken cancellationToken);
        Task<GetStaffResponse> GetAsync(CancellationToken cancellationToken, bool asc = true, int offset = 0, int take = 5, string? orderBy = null, string? search = null, int[]? excludeRole = null);
        Task<StaffDto[]> GetAsync(int[] id, CancellationToken cancellationToken);
        Task<StaffDto?> GetAsync(int id, CancellationToken cancellationToken);
        Task<StaffExportModel[]> GetExportModelsAsync(int[]? id, CancellationToken cancellationToken);
        Task<bool> RoleExistsAsync(int id, CancellationToken cancellationToken);
        Task<StaffRoleDto[]> GetRolesAsync(CancellationToken cancellationToken);
    }
}
