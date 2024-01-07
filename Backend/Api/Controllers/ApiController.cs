using Domain.Core.Primitives;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly ILogger logger;

        public ApiController(ILogger<ApiController> logger)
        {
            this.logger = logger;
        }
        protected IActionResult Problem(Error error)
        {
            //logger.LogError("Error occured: {0}", Request.Path);
            return Problem(statusCode: error.statusCode, title: error.message);
        }
    }
}
