using Api.Controllers.Base;
using Application.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IUserService userService;

        public UsersController(ILogger<ApiController> logger, IUserService userService) : base(logger)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserByEmail(string email, CancellationToken cancellationToken)
        {
            var result = await userService.CheckIfUserExistsAsync(email, cancellationToken);
            return result.isSuccess ? Ok(result.value) : Problem(result.error);
        }                
    }
}
