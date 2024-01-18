using Domain.Core.Primitives;
using Infrastructure;
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
            logger.LogWarning("Error occured: {0}\n{1}", Request.Path, error.logMessage);
            return Problem(statusCode: (int)error.statusCode, title: error.message);
        }
    }
}
