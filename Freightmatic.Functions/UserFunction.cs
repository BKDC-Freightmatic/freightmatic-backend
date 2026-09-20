using Freightmatic.Application.Shared;
using Freightmatic.Application.Users;
using Freightmatic.Functions.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Freightmatic.Functions
{
    public class UserFunction
    {
        private readonly UserAppService _userAppService;
        private readonly IConfiguration _configuration;
        private readonly JwtValidation _jwtValidation;

        public UserFunction(UserAppService userAppService, IConfiguration configuration, JwtValidation jwtValidation)
        {
            _userAppService = userAppService;
            _configuration = configuration;
            _jwtValidation = jwtValidation;
        }

        [FunctionName("profile")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "profile")] UpdateUserDto data, HttpRequest req, ILogger log)
        {
            try
            {
                var isValid = _jwtValidation.ValidateJwtAccessToken(req);
                if (!isValid)
                    throw new CustomException("Unauthorized", "Unauthorized", HttpStatusCode.Unauthorized);

                data.id = _jwtValidation.GetUserIdFromToken();
                EmptyReponse result = await _userAppService.UpdateAsync(data);
                return new OkObjectResult(new { });
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

        [FunctionName("top-truckers")]
        public async Task<IActionResult> GetTopTrucker([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/top-truckers")] HttpRequest req, ILogger log)
        {
            try
            {
                var isValid = _jwtValidation.ValidateJwtAccessToken(req);
                if (!isValid)
                    throw new CustomException("Unauthorized", "Unauthorized", HttpStatusCode.Unauthorized);

                IEnumerable<UserDto> result = await _userAppService.GetTopTruckers();
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

        [FunctionName("suggest-truckers")]
        public async Task<IActionResult> GetSuggestTrucker([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/suggest-truckers")] HttpRequest req, ILogger log)
        {
            try
            {
                var isValid = _jwtValidation.ValidateJwtAccessToken(req);
                if (!isValid)
                    throw new CustomException("Unauthorized", "Unauthorized", HttpStatusCode.Unauthorized);

                IEnumerable<UserDto> result = await _userAppService.GetSuggestTruckers();
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

        [FunctionName("get-user-by-id")]
        public async Task<IActionResult> GetUser([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "profile/{userId}")] HttpRequest req, string userId, ILogger log)
        {
            try
            {
                var isValid = _jwtValidation.ValidateJwtAccessToken(req);
                if (!isValid)
                    throw new CustomException("Unauthorized", "Unauthorized", HttpStatusCode.Unauthorized);

                log.LogInformation($"call with user Id: {userId}");

                UserDto result = await _userAppService.GetById(userId);
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
