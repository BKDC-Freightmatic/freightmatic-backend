using Freightmatic.Application.Delivery;
using Freightmatic.Application.News;
using Freightmatic.Application.Shared;
using Freightmatic.Functions.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Freightmatic.Functions
{
    public class DeliveryFunction
    {
        private readonly DeliveryAppService _deliveryAppService;
        private readonly IConfiguration _configuration;
        private readonly JwtValidation _jwtValidation;

        public DeliveryFunction(DeliveryAppService deliveryAppService, IConfiguration configuration, JwtValidation jwtValidation)
        {
            _deliveryAppService = deliveryAppService;
            _configuration = configuration;
            _jwtValidation = jwtValidation;
        }

        [FunctionName("create-delivery")]
        public async Task<IActionResult> Create([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "deliveries")] CreateDeliveryDto dto, HttpRequest req, ILogger log)
        {
            try
            {
                var isValid = _jwtValidation.ValidateJwtAccessToken(req);
                if (!isValid)
                    throw new CustomException("Unauthorized", "Unauthorized", HttpStatusCode.Unauthorized);

                string userId = _jwtValidation.GetUserIdFromToken();
                DeliveryDto result = await _deliveryAppService.Create(userId, dto);
                return new OkObjectResult(result);
            }
            catch (CustomException ex)
            {
                var response = new ObjectResult(new ErrorResponse(ex.Message, ex.Error, ex.StatusCode))
                {
                    StatusCode = (int?)ex.StatusCode
                };
                return response;
            }
            catch (System.Exception ex)
            {
                log.LogError(ex.StackTrace);
                var response = new ObjectResult(new ErrorResponse())
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
                return response;
            }
        }

        [FunctionName("get-delivery-by-id")]
        public async Task<IActionResult> GetById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "deliveries/{deliveryId}")] HttpRequest req, string deliveryId, ILogger log)
        {
            try
            {
                var isValid = _jwtValidation.ValidateJwtAccessToken(req);
                if (!isValid)
                    throw new CustomException("Unauthorized", "Unauthorized", HttpStatusCode.Unauthorized);

                DeliveryDto result = await _deliveryAppService.GetById(deliveryId);
                return new OkObjectResult(result);
            }
            catch (CustomException ex)
            {
                var response = new ObjectResult(new ErrorResponse(ex.Message, ex.Error, ex.StatusCode))
                {
                    StatusCode = (int?)ex.StatusCode
                };
                return response;
            }
            catch (System.Exception ex)
            {
                log.LogError(ex.StackTrace);
                var response = new ObjectResult(new ErrorResponse())
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
                return response;
            }
        }

        [FunctionName("search-deliveries")]
        public async Task<IActionResult> Search([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "deliveries")] SearchDeliveryDto searchParams, HttpRequest req, ILogger log)
        {
            try
            {
                var isValid = _jwtValidation.ValidateJwtAccessToken(req);
                if (!isValid)
                    throw new CustomException("Unauthorized", "Unauthorized", HttpStatusCode.Unauthorized);

                log.LogInformation($"call with serach param: {JsonSerializer.Serialize(searchParams)}");

                IEnumerable<DeliveryDto> result = await _deliveryAppService.Search(searchParams);
                return new OkObjectResult(result);
            }
            catch (CustomException ex)
            {
                var response = new ObjectResult(new ErrorResponse(ex.Message, ex.Error, ex.StatusCode))
                {
                    StatusCode = (int?)ex.StatusCode
                };
                return response;
            }
            catch (System.Exception ex)
            {
                log.LogError(ex.StackTrace);
                var response = new ObjectResult(new ErrorResponse())
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
                return response;
            }
        }

        [FunctionName("update-delivery-status")]
        public async Task<IActionResult> UpdateStatus(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "deliveries/{deliveryId}/status")] UpdateDeliveryStatusDto dto,
            string deliveryId,
            HttpRequest req,
            ILogger log)
        {
            try
            {
                var isValid = _jwtValidation.ValidateJwtAccessToken(req);
                if (!isValid)
                    throw new CustomException("Unauthorized", "Unauthorized", HttpStatusCode.Unauthorized);

                await _deliveryAppService.UpdateDeliveryStatus(deliveryId, dto.Status);
                return new OkObjectResult(new {});
            }
            catch (CustomException ex)
            {
                var response = new ObjectResult(new ErrorResponse(ex.Message, ex.Error, ex.StatusCode))
                {
                    StatusCode = (int?)ex.StatusCode
                };
                return response;
            }
            catch (System.Exception ex)
            {
                log.LogError(ex.StackTrace);
                var response = new ObjectResult(new ErrorResponse())
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
                return response;
            }
        }

        [FunctionName("update-delivery-step")]
        public async Task<IActionResult> UpdateDeliveryStep(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "deliveries/{deliveryId}/step")] UpdateDeliveryStepDto dto,
            string deliveryId,
            HttpRequest req,
            ILogger log)
        {
            try
            {
                var isValid = _jwtValidation.ValidateJwtAccessToken(req);
                if (!isValid)
                    throw new CustomException("Unauthorized", "Unauthorized", HttpStatusCode.Unauthorized);

                await _deliveryAppService.UpdateDeliveryStep(deliveryId, dto);
                return new OkObjectResult(new {});
            }
            catch (CustomException ex)
            {
                var response = new ObjectResult(new ErrorResponse(ex.Message, ex.Error, ex.StatusCode))
                {
                    StatusCode = (int?)ex.StatusCode
                };
                return response;
            }
            catch (System.Exception ex)
            {
                log.LogError(ex.StackTrace);
                var response = new ObjectResult(new ErrorResponse())
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
                return response;
            }
        }
    }
}
