using Api.Controllers.Base;
using Domain.Enumeration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class StaffController : ApiController
    {
        public StaffController(ILogger<ApiController> logger) : base(logger)
        {
        }

        [HttpPost, Authorize(Roles = Roles.Permissions.createStaff)]
        public async Task<IActionResult> CreateStaff(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpGet, Authorize(Roles = Roles.Permissions.readStaff)]
        public async Task<IActionResult> GetStaff(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id:int}"), Authorize(Roles =Roles.Permissions.readStaff)]
        public async Task<IActionResult> GetStaff(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpGet("export"), Authorize(Roles =Roles.Permissions.readStaff)]
        public async Task<IActionResult> Export(int[]? ids, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id:int}"), Authorize(Roles = Roles.Permissions.updateStaff)]
        public async Task<IActionResult> UpdateStaff(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id:int}"), Authorize(Roles =Roles.Permissions.deleteStaff)]
        public async Task<IActionResult> DeleteStaff(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
