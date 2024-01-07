using Microsoft.AspNetCore.Mvc;
using Contracts.User;
using Domain.Core.Primitives;
using Microsoft.AspNetCore.Http.HttpResults;
using Domain.Enumeration;
using Domain.Core.Utility;
using Api.Controllers.Extensions;

namespace Api.Controllers
{

    public class AuthenticationController : ApiController
    {
        public AuthenticationController(ILogger<ApiController> logger) : base(logger)
        {
        }

        //TODO: Check if user exists, Register, LogIn, LogOut, Refresh token
        [HttpGet("user/{email}")]
        public IActionResult GetUserByEmail(string email)
        {
            if (!Ensure.isEmail(email))
            {
                return Problem(CommonErrors.User.InvalidEmail);
            }
            return Ok();
        }
    }
}
