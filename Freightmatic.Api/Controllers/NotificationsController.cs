using Freightmatic.Application.Notifications;
using Freightmatic.Application.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Freightmatic.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly NotificationAppService _notificationAppService;

        public NotificationsController(NotificationAppService notificationAppService)
        {
            _notificationAppService = notificationAppService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                IEnumerable<NotificationDto> result = await _notificationAppService.GetAll();
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
