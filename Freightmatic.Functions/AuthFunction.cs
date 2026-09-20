using Freightmatic.Application.Shared;
using Freightmatic.Application.Users;
using Freightmatic.Functions.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace Freightmatic.Functions
{
    public class AuthFunction
    {
        private readonly UserAppService _userAppService;
        private readonly IConfiguration _configuration;
        private readonly JwtValidation _jwtValidation;

        public AuthFunction(UserAppService userAppService, IConfiguration configuration, JwtValidation jwtValidation)
        {
            _userAppService = userAppService;
            _configuration = configuration;
            _jwtValidation = jwtValidation;
        }

        [FunctionName("sign-in")]
        public async Task<IActionResult> RunSignIn([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth/sign-in")] LoginUserDto data, ILogger log)
        {
            try
            {
                AuthDto result = await _userAppService.LoginAsync(data.Username, data.Password);
                return new ObjectResult(result);
            }
            catch (CustomException ex)
            {
                return new BadRequestObjectResult(new ErrorResponse(ex.Message, ex.Error, HttpStatusCode.BadRequest));
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

        [FunctionName("sign-up")]
        public async Task<IActionResult> RunSignUp([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth/sign-up")] CreateUserDto data, ILogger log)
        {
            try
            {
                AuthDto result = await _userAppService.CreateAsync(data);
                return new ObjectResult(result);
            }
            catch (CustomException ex)
            {
                return new BadRequestObjectResult(new ErrorResponse(ex.Message, ex.Error, HttpStatusCode.BadRequest));
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

        [FunctionName("sign-out")]
        public async Task<IActionResult> RunSignOut([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth/sign-out")] HttpRequest req, ILogger log)
        {
            try
            {
                var isValid = _jwtValidation.ValidateJwtAccessToken(req);
                if (!isValid)
                    throw new CustomException("Unauthorized", "Unauthorized", HttpStatusCode.Unauthorized);

                string userId = _jwtValidation.GetUserIdFromToken();
                EmptyReponse result = await _userAppService.LogoutAsync(userId);

                return new OkObjectResult(new {});
            }
            catch (CustomException ex)
            {
                return new ObjectResult(new ErrorResponse(ex.Message, ex.Error, ex.StatusCode));
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

        [FunctionName("hello-world")]
        public async Task<IActionResult> RunTest([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "auth/hello-world")] HttpRequest req, ILogger log)
        {
            log.LogInformation("This is test endpoint");
            return new OkResult();
        }
    }
}