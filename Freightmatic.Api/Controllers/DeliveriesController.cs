using Freightmatic.Application.Delivery;
using Freightmatic.Application.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Freightmatic.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("deliveries")]
    public class DeliveriesController : ControllerBase
    {
        private readonly DeliveryAppService _deliveryAppService;

        public DeliveriesController(DeliveryAppService deliveryAppService)
        {
            _deliveryAppService = deliveryAppService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDeliveryDto dto)
        {
            try
            {
                string userId = User.FindFirstValue("sub") ?? User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                DeliveryDto result = await _deliveryAppService.Create(userId, dto);
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

        [HttpGet("{deliveryId}")]
        public async Task<IActionResult> GetById(string deliveryId)
        {
            try
            {
                DeliveryDto result = await _deliveryAppService.GetById(deliveryId);
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

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] SearchDeliveryDto searchParams)
        {
            try
            {
                IEnumerable<DeliveryDto> result = await _deliveryAppService.Search(searchParams);
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

        [HttpPut("{deliveryId}/status")]
        public async Task<IActionResult> UpdateStatus(string deliveryId, [FromBody] UpdateDeliveryStatusDto dto)
        {
            try
            {
                await _deliveryAppService.UpdateDeliveryStatus(deliveryId, dto.Status);
                return Ok(new { });
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

        [HttpPut("{deliveryId}/step")]
        public async Task<IActionResult> UpdateStep(string deliveryId, [FromBody] UpdateDeliveryStepDto dto)
        {
            try
            {
                await _deliveryAppService.UpdateDeliveryStep(deliveryId, dto);
                return Ok(new { });
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
