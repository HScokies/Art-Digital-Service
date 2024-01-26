using Api.Controllers.Base;
using Application.Services.Case;
using Contracts.Cases;
using Domain.Enumeration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class CasesController : ApiController
    {
        private readonly ICaseService caseService;

        public CasesController(ILogger<ApiController> logger, ICaseService caseService) : base(logger)
        {
            this.caseService = caseService;
        }       

        [HttpGet]
        public IActionResult Get(string? search = null)
        {
            var res = caseService.Get(search);
            return Ok(res);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var Result = await caseService.GetAsync(id, cancellationToken);
            return Result.isSuccess ? Ok(Result.value) : Problem(Result.error);
        }

        [HttpPost, Authorize(Roles = Roles.Permissions.createCases)]
        public async Task<IActionResult> Create([FromForm] UpsertCaseRequest request, CancellationToken cancellationToken)
        {
            var entityId = await caseService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(Create), entityId);
        }

        [HttpPut("{id:int}"), Authorize(Roles = Roles.Permissions.updateCases)]
        public async Task<IActionResult> Update(int id, [FromForm] UpsertCaseRequest request, CancellationToken cancellationToken)
        {
            var Result = await caseService.UpdateAsync(id, request, cancellationToken);
            return Result.isSuccess? NoContent() : Problem(Result.error);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var Result = await caseService.DropAsync(id, cancellationToken);
            return Result.isSuccess ? NoContent() : Problem(Result.error);
        }
    }
}
