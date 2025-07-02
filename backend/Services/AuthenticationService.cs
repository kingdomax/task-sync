using TaskSync.Models.Dto;
using TaskSync.Repositories.Entities;
using TaskSync.Repositories.Interfaces;
using TaskSync.Services.Interfaces;

using IAuthenticationService = TaskSync.Services.Interfaces.IAuthenticationService;

namespace TaskSync.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtService _jwtService;
        private readonly IRepository<UserEntity> _userRepository; // todo-moch: need proper repository interface

        public AuthenticationService(IJwtService jwtService, IRepository<UserEntity> userRepository)
        {
            _jwtService = jwtService;
            _userRepository = userRepository;
        }

        public async Task<string?> AuthenticateAsync(LoginRequest request)
        {
            var user = await _userRepository.GetAsync(request.Email);

            // string passwordHash = BCrypt.Net.BCrypt.HashPassword("mypassword");
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return null;
            }

            return _jwtService.GenerateJwtToken(user);
        }
    }
}
