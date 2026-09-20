using Freightmatic.Application.Shared;
using Freightmatic.Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Freightmatic.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserAppService _userAppService;

        public AuthController(UserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] LoginUserDto data)
        {
            try
            {
                AuthDto result = await _userAppService.LoginAsync(data.Username, data.Password);
                return Ok(result);
            }
            catch (CustomException ex)
            {
                return BadRequest(new ErrorResponse(ex.Message, ex.Error, HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse());
            }
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] CreateUserDto data)
        {
            try
            {
                AuthDto result = await _userAppService.CreateAsync(data);
                return Ok(result);
            }
            catch (CustomException ex)
            {
                return BadRequest(new ErrorResponse(ex.Message, ex.Error, HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse());
            }
        }

        [Authorize]
        [HttpPost("sign-out")]
        public async Task<IActionResult> SignOutUser()
        {
            try
            {
                string userId = User.FindFirstValue("sub") ?? User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                EmptyReponse result = await _userAppService.LogoutAsync(userId);
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

        [HttpGet("hello-world")]
        public IActionResult HelloWorld()
        {
            return Ok("Hello World from Freightmatic API!");
        }
    }
}
