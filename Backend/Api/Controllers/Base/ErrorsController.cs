using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Base
{
    public class ErrorsController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Error()
        {
            return Problem();
        }
    }
}
