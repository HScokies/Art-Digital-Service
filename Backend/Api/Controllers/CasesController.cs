using Api.Controllers.Base;
using Domain.Enumeration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class CasesController : ApiController
    {
        public CasesController(ILogger<ApiController> logger) : base(logger)
        {

        }
        
        [HttpPost, Authorize(Roles = Roles.Permissions.createCases)]
        public IActionResult Create() => throw new NotImplementedException();

        [HttpGet]
        public IActionResult Get() => throw new NotImplementedException();

        [HttpGet("{id:int}")]
        public IActionResult Get(int id) => throw new NotImplementedException();

        [HttpPut("{id:int}"), Authorize(Roles = Roles.Permissions.updateCases)]
        public IActionResult Update(int id) => throw new NotImplementedException();

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id) => throw new NotImplementedException();
    }
}
