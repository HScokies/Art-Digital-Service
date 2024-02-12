using Contracts.File;
using Domain.Core.Primitives;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;


namespace Api.Controllers.Base
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

        protected FileStreamResult File(FileResponse file)
        {
            var ext = GetExt(file.contentType);
            return File(file.fileStream, file.contentType, $"{file.fileName}{ext}");
        }

        private string GetExt(string contentType)
        {
            return new FileExtensionContentTypeProvider().Mappings.First(v => v.Value == contentType).Key;
        }
    }
}
