using Domain.Core.Primitives;
using Domain.Enumeration;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers.Extensions
{
    public static class ControllersExtension
    {
        public static IActionResult ToOkObjectResult<TResult>(this Result<TResult> res, Func<Error,IActionResult> toProblem)
        {
            return res.Match(
                value =>
                {
                    return new OkObjectResult(value);
                },
                error =>
                {
                    return toProblem(error);
                }
            );
        }

        public static Result<int> GetUserId(this ClaimsPrincipal User)
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (claim is null)
                return new Result<int>(CommonErrors.User.InvalidToken);

            if (!int.TryParse(claim.Value, out int id))
                return new Result<int>(CommonErrors.User.InvalidToken);

            return new Result<int>(id);
        }
    }
}
