using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TaskSync.Models.Dto;
using TaskSync.Services.Interfaces;

namespace TaskSync.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

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
            var username = User.FindFirstValue(ClaimTypes.Name);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Ok(new
            {
                userId,
                username,
            });
        }
    }
}
