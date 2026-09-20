using Freightmatic.Application.News;
using Freightmatic.Application.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Freightmatic.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("news")]
    public class NewsController : ControllerBase
    {
        private readonly NewsAppService _newsAppService;

        public NewsController(NewsAppService newsAppService)
        {
            _newsAppService = newsAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                IEnumerable<NewsDto> result = await _newsAppService.GetAll();
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
