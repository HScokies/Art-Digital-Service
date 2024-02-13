using Api.Controllers.Base;
using Application.Services.Staff;
using Contracts.Staff;
using Domain.Enumeration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    public class StaffController : ApiController
    {
        private readonly IStaffService staffService;

        public StaffController(ILogger<ApiController> logger, IStaffService staffService) : base(logger)
        {
            this.staffService = staffService;
        }

        [HttpPost, Authorize(Roles = Roles.Permissions.createStaff)]
        public async Task<IActionResult> CreateStaff([FromForm] CreateStaffRequest request,CancellationToken cancellationToken)
        {
            var Result = await staffService.CreateAsync(request, cancellationToken);
            return Result.isSuccess? CreatedAtAction(nameof(CreateStaff), Result.value) : Problem(Result.error);
        }

        [HttpGet("roles"), Authorize(Roles = Roles.Permissions.readStaff)]
        public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
        {
            var res = await staffService.GetStaffRolesAsync(cancellationToken);
            return Ok(res);
        }

        [HttpGet, Authorize(Roles = Roles.Permissions.readStaff)]
        public async Task<IActionResult> GetStaff(CancellationToken cancellationToken, bool asc = true, int offset = 0, int take = 5, string? orderBy = null, string? search = null, [FromQuery] int[]? excludeRole = null)
        {
            var res = await staffService.GetStaffAsync(cancellationToken, offset, take, asc, orderBy, search, excludeRole);
            return Ok(res);
        }

        [HttpGet("{id:int}"), Authorize(Roles =Roles.Permissions.readStaff)]
        public async Task<IActionResult> GetStaff(int id, CancellationToken cancellationToken)
        {
            var Result = await staffService.GetStaffAsync(id, cancellationToken);
            return Result.isSuccess ? Ok(Result.value) : Problem(Result.error);
        }

        [HttpPut("{id:int}"), Authorize(Roles = Roles.Permissions.updateStaff)]
        public async Task<IActionResult> UpdateStaff(int id,[FromForm] UpdateStaffRequest request, CancellationToken cancellationToken)
        {
            var Result = await staffService.UpdateStaffAsync(id, request, cancellationToken);
            return Result.isSuccess ? NoContent() : Problem(Result.error);
        }

        [HttpDelete, Authorize(Roles = Roles.Permissions.deleteStaff)]
        public async Task<IActionResult> DeleteStaff([FromQuery] int[] id, CancellationToken cancellationToken)
        {
            await staffService.DropStaffAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpGet("permissions"), Authorize]
        public IActionResult GetPermissions()
        {
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).ToArray();
            Dictionary<string, bool> permissions = new()
            {
                {"readUsers", roles.Any(r => r.Value == Roles.Permissions.readUsers) },
                {"createUsers", roles.Any(r => r.Value == Roles.Permissions.createUsers) },
                {"updateUsers", roles.Any(r => r.Value == Roles.Permissions.updateUsers) },
                {"rateUsers", roles.Any(r => r.Value == Roles.Permissions.rateUsers) },
                {"deleteUsers", roles.Any(r => r.Value == Roles.Permissions.deleteUsers) },

                {"readStaff", roles.Any(r => r.Value == Roles.Permissions.readStaff) },
                {"createStaff", roles.Any(r => r.Value == Roles.Permissions.createStaff) },
                {"updateStaff", roles.Any(r => r.Value == Roles.Permissions.updateStaff) },
                {"deleteStaff", roles.Any(r => r.Value == Roles.Permissions.deleteStaff) },

                {"readCases", roles.Any(r => r.Value == Roles.Permissions.readCases) },
                {"createCases", roles.Any(r => r.Value == Roles.Permissions.createCases) },
                {"updateCases", roles.Any(r => r.Value == Roles.Permissions.updateCases) },
                {"deleteCases", roles.Any(r => r.Value == Roles.Permissions.deleteCases) },

                {"utilsAccess", roles.Any(r => r.Value == Roles.Permissions.utilsAccess) }
            };

            return Ok(permissions);
        }

        [HttpGet("export")]//, Authorize(Roles = Roles.Permissions.readStaff)
        public async Task<IActionResult> Export([FromQuery] int[]? id, CancellationToken cancellationToken)
        {
            var res = await staffService.ExportStaffAsync(id, cancellationToken);
            res.fileName = $"Сотрудники {DateTime.UtcNow}";
            return File(res);
        }
    }
}
