using Bcrypt = BCrypt.Net.BCrypt;
using Contracts.Staff;
using Data.Interfaces;
using Domain.Core.Primitives;
using Domain.Core.Utility;
using Domain.Enumeration;
using Domain.Entities;
using Contracts;
using Domain.Repositories;
using Infrastructure.Export;
using Contracts.File;


namespace Application.Services.Staff
{
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository staffRepository;
        private readonly IRepository repository;
        private readonly IExportProvider exportProvider;

        public StaffService(IStaffRepository staffRepository, IRepository repository, IExportProvider exportProvider)
        {
            this.staffRepository = staffRepository;
            this.repository = repository;
            this.exportProvider = exportProvider;
        }

        public async Task<Result<int>> CreateAsync(CreateStaffRequest request, CancellationToken cancellationToken)
        {
            if (!Ensure.isEmail(request.email))
                return new Result<int>(CommonErrors.User.InvalidEmail);
            if (!Ensure.isPassword(request.password))
                return new Result<int>(CommonErrors.User.InvalidPassword);

            var roleExists = await staffRepository.RoleExistsAsync(request.roleId, cancellationToken);
            if (!roleExists)
                return new Result<int>(CommonErrors.Staff.InvalidRole);

            request.password = Bcrypt.EnhancedHashPassword(request.password);
            var staff = request.toStaff();
            var res = await staffRepository.CreateAsync(staff, cancellationToken);
            return new Result<int>(res.id);
        }

        public async Task DropStaffAsync(int[] id, CancellationToken cancellationToken)
        {
            var staff = await staffRepository.GetAsync(id, cancellationToken);
            await staffRepository.DropAsync(staff, cancellationToken);
        }

        public async Task<FileResponse> ExportStaffAsync(int[]? id, CancellationToken cancellationToken)
        {
            var staff = await staffRepository.GetExportModelsAsync(id, cancellationToken);
            return exportProvider.ExportStaff(staff);
        }

        public async Task<GetStaffResponse> GetStaffAsync(CancellationToken cancellationToken, int offset, int take, bool asc = true, string? orderBy = null, string? search = null, int[]? excludeRole = null) => 
            await staffRepository.GetAsync(cancellationToken, asc, offset, take, orderBy, search, excludeRole);

        public async Task<Result<GetStaffMemberResponse>> GetStaffAsync(int id, CancellationToken cancellationToken)
        {
            var res = await staffRepository.GetAsync(id, cancellationToken);
            if (res is null)
                return new Result<GetStaffMemberResponse>(CommonErrors.User.NotFound);
            return new Result<GetStaffMemberResponse>(res.toStaffResponse());
        }

        public async Task<StaffRoleDto[]> GetStaffRolesAsync(CancellationToken cancellationToken) => await staffRepository.GetRolesAsync(cancellationToken);

        public async Task<Result<bool>> UpdateStaffAsync(int id, UpdateStaffRequest request, CancellationToken cancellationToken)
        {
            if (!Ensure.isEmail(request.email))
                return new Result<bool>(CommonErrors.User.InvalidEmail);

            var staff = await staffRepository.GetAsync(id, cancellationToken);
            if (staff is null)
                return new Result<bool>(CommonErrors.User.NotFound);

            staff.updateStaff(request);
            await repository.SaveChangesAsync(cancellationToken);
            return new Result<bool>();
        }

    }
}
