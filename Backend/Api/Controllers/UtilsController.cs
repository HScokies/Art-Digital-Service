using Api.Controllers.Base;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class UtilsController : ApiController
    {
        private readonly IRepository repository;

        public UtilsController(ILogger<ApiController> logger, IRepository repository) : base(logger)
        {
            this.repository = repository;
        }

        [HttpGet("cities")]
        public async Task<IActionResult> GetCities(CancellationToken cancellationToken)
        {
            var res = await repository.GetAsync<CityDto>(cancellationToken);
            return Ok(res);
        }

    }
}
