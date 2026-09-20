using Freightmatic.Application.Files;
using Freightmatic.Application.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Freightmatic.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("files")]
    public class FilesController : ControllerBase
    {
        private readonly FileAppService _fileAppService;

        public FilesController(FileAppService fileAppService)
        {
            _fileAppService = fileAppService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles([FromForm] IFormFileCollection files)
        {
            try
            {
                if (files == null || files.Count == 0)
                    throw new CustomException("No files were uploaded", "Bad Request", HttpStatusCode.BadRequest);

                List<FileUploadDto> fileUploadDtos = new();
                foreach (var file in files)
                {
                    using var ms = new MemoryStream();
                    await file.CopyToAsync(ms);
                    fileUploadDtos.Add(new FileUploadDto { Content = ms.ToArray(), Title = file.FileName });
                }

                List<Freightmatic.Application.Files.FileDto> result = await _fileAppService.Upload(fileUploadDtos);
                return Ok(result);
            }
            catch (CustomException ex)
            {
                return StatusCode((int)ex.StatusCode, new ErrorResponse(ex.Message, ex.Error, ex.StatusCode));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse());
            }
        }
    }
}
