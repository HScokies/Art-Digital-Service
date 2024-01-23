using Contracts.File;
using Contracts.Staff;
using Domain.Core.Primitives;
using Domain.Entities;

namespace Application.Services.Staff
{
    public interface IStaffService
    {
        Task<Result<int>> CreateAsync(CreateStaffRequest request, CancellationToken cancellationToken);
        Task<GetStaffResponse> GetStaffAsync(CancellationToken cancellationToken, int offset, int take, bool asc = true, string? orderBy = null, string? search = null, int[]? excludeRole = null);
        Task <Result<GetStaffMemberResponse>> GetStaffAsync(int id, CancellationToken cancellationToken);
        Task<StaffRoleDto[]> GetStaffRolesAsync(CancellationToken cancellationToken);
        Task DropStaffAsync(int[] id, CancellationToken cancellationToken);
        Task<Result<bool>> UpdateStaffAsync(int id, UpdateStaffRequest request, CancellationToken cancellationToken);
        Task<FileResponse> ExportStaffAsync(int[]? id, CancellationToken cancellationToken);
    }
}
