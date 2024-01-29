using Contracts.Staff;
using Contracts.User;
using Data.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Data.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly AppDbContext ctx;

        public StaffRepository(AppDbContext ctx) => this.ctx = ctx;

        public async Task<StaffDto> CreateAsync(StaffDto staff, CancellationToken cancellationToken)
        {
            var res = await ctx.staff.AddAsync(staff, cancellationToken);
            await ctx.SaveChangesAsync(cancellationToken);
            return res.Entity;
        }


        public async Task<GetStaffResponse> GetAsync(CancellationToken cancellationToken, bool asc = true, int offset = 0, int take =5, string? orderBy = null, string? search = null, int[]? excludeRole = null)
        {
            var query = ctx.staff.AsQueryable();
            if (!search.IsNullOrEmpty())
            {
                query = query.Where(s => EF.Functions.ILike(s.User.lastName + " " + s.User.firstName + " " + s.User.patronymic, $"%{search}%"));
            }
            if (excludeRole?.Length > 0)
            {
                query = query.Where(s => !excludeRole.Contains(s.roleId));
            }

            if (orderBy == "name")
                query = asc ? query.OrderBy(s => s.User.lastName+" "+s.User.firstName+" "+s.User.patronymic) : query.OrderByDescending(s => s.User.lastName + " " + s.User.firstName + " " + s.User.patronymic);
            if (orderBy == "role")
                query = asc ? query.OrderBy(s => s.Role.name) : query.OrderByDescending( s => s.Role.name);

            var staff = query.Select(s => new StaffPreview()
            {
                id = s.id,
                fullName = $"{s.User.lastName} {s.User.firstName} {s.User.patronymic}",
                roleName = s.Role.name

            });

            int currentPage = offset > 0 ? offset / take + 1 : 1; // Запрашиваемая страница
            int pageCount = (int)Math.Ceiling((double)staff.Count() / take); //Всего страниц доступно
            currentPage = currentPage > pageCount ? pageCount : currentPage; // Обновляем текущую страницу если нужно

            var response = new GetStaffResponse()
            {
                currentPage = currentPage,
                pagesTotal = pageCount
            };
            if (pageCount < 1)
                return response;

            response.rows = await staff.Skip((currentPage - 1) * take).Take(take).ToArrayAsync(cancellationToken);
            return response;
        }

        public async Task<StaffDto?> GetAsync(int id, CancellationToken cancellationToken) => await ctx.staff.Include(s => s.User).FirstOrDefaultAsync(s => s.id == id, cancellationToken);

        public async Task<StaffDto[]> GetAsync(int[] id, CancellationToken cancellationToken) => await ctx.staff.Include(s => s.User).Where(s => id.Contains(s.id)).ToArrayAsync(cancellationToken);

        public async Task<StaffExportModel[]> GetExportModelsAsync(int[]? id, CancellationToken cancellationToken)
        {
            var staff = ctx.staff.AsQueryable();
            if (id?.Length > 0)
            {
                staff = staff.Where(s => id.Contains(s.id));
            }
            return await staff.Select(s => new StaffExportModel()
            {
                email = s.User.email,
                firstName = s.User.firstName,
                lastName = s.User.lastName,
                patronymic = s.User.patronymic,
                role = s.Role.name
            }).ToArrayAsync(cancellationToken);
        }

        public async Task<StaffRoleDto[]> GetRolesAsync(CancellationToken cancellationToken) => await ctx.roles.ToArrayAsync(cancellationToken);

        public async Task<bool> RoleExistsAsync(int id, CancellationToken cancellationToken) => await ctx.roles.AnyAsync(r => r.id == id, cancellationToken);
    }
}
