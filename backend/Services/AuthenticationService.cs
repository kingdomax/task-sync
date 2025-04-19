using TaskSync.Models.Dto;
using TaskSync.Services.Interfaces;
using IAuthenticationService = TaskSync.Services.Interfaces.IAuthenticationService;

namespace TaskSync.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;

        public AuthenticationService(IJwtService jwtService, IUserService userService)
        {
            _jwtService = jwtService;
            _userService = userService;
        }

        public async Task<string?> AuthenticateAsync(LoginRequest request)
        {
            var user = await _userService.GetUserAsync(request.Email);

            //string passwordHash = BCrypt.Net.BCrypt.HashPassword("mypassword");
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return null;
            }

            return _jwtService.GenerateJwtToken(user);
        }
    }
}
