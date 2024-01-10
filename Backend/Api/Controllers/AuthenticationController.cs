using Domain.Core.Utility;
using Domain.Enumeration;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{

    public class AuthenticationController : ApiController
    {
        private readonly IUserRepository users;

        public AuthenticationController(ILogger<ApiController> logger, IUserRepository users) : base(logger)
        {
            this.users = users;
        }

        //TODO: Check if user exists, Register, LogIn, LogOut, Refresh token
    }
}
