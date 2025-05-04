using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskSync.Filters;
using TaskSync.Models.Dto;
using TaskSync.Services.Interfaces;

namespace TaskSync.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IUserService userService, IAuthenticationService authenticationService)
        {
            _userService = userService;
            _authenticationService = authenticationService;
        }

        [ValidateRequest]
        [HttpPost("test")]
        public async Task<IActionResult> Test([FromBody] TestRequest request)
        {
            Console.WriteLine("In 'auth/test' controller");

            var result = await _userService.GetUserAsync();

            return Ok(result);
        }

        [ValidateRequest]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _authenticationService.AuthenticateAsync(request);
            return token != null ? Ok(new LoginResponse { Token = token }) : Unauthorized("Invalid credentials");
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);

            return Ok(new
            {
                userId,
                email
            });
        }
    }
}