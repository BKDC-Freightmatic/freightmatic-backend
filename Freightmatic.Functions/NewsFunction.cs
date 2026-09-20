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
using System.Threading.Tasks;

namespace Freightmatic.Functions
{
    public class NewsFunction
    {
        private readonly NewsAppService _newsAppService;
        private readonly IConfiguration _configuration;
        private readonly JwtValidation _jwtValidation;

        public NewsFunction(NewsAppService newsAppService, IConfiguration configuration, JwtValidation jwtValidation)
        {
            _newsAppService = newsAppService;
            _configuration = configuration;
            _jwtValidation = jwtValidation;
        }

        [FunctionName("news")]
        public async Task<IActionResult> GetAll([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "news")] HttpRequest req, ILogger log)
        {
            try
            {
                var isValid = _jwtValidation.ValidateJwtAccessToken(req);
                if (!isValid)
                    throw new CustomException("Unauthorized", "Unauthorized", HttpStatusCode.Unauthorized);

                IEnumerable<NewsDto> result = await _newsAppService.GetAll();
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
    }
}
