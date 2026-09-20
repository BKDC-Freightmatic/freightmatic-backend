using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Freightmatic.Application.News;
using Freightmatic.Application.Shared;
using Freightmatic.Functions.Shared;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net;
using Freightmatic.Application.Notifications;

namespace Freightmatic.Functions
{
    public class NotificationFunction
    {
        private readonly NotificationAppService _notificationAppService;
        private readonly IConfiguration _configuration;
        private readonly JwtValidation _jwtValidation;

        public NotificationFunction(NotificationAppService notificationAppService, IConfiguration configuration, JwtValidation jwtValidation)
        {
            _notificationAppService = notificationAppService;
            _configuration = configuration;
            _jwtValidation = jwtValidation;
        }

        [FunctionName("notifications")]
        public async Task<IActionResult> GetAll([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "notifications")] HttpRequest req, ILogger log)
        {
            try
            {
                var isValid = _jwtValidation.ValidateJwtAccessToken(req);
                if (!isValid)
                    throw new CustomException("Unauthorized", "Unauthorized", HttpStatusCode.Unauthorized);

                IEnumerable<NotificationDto> result = await _notificationAppService.GetAll();
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
            catch (Exception ex)
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
