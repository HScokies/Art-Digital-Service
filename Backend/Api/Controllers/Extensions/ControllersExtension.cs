using Domain.Core.Primitives;
using Microsoft.AspNetCore.Mvc;

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
    }
}
