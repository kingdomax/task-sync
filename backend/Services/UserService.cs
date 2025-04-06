using TaskSync.Repositories.Entity;
using TaskSync.Repositories;
using TaskSync.Services.Interfaces;
using TaskSync.Models;

namespace TaskSync.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<UserEntity> _userRepository;

        public UserService(IRepository<UserEntity> userRepository) 
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetUserAsync()
        {
            var userDto = await _userRepository.GetAsync();

            return userDto != null ? new User(userDto.Username, userDto.Firstname, userDto.Lastname, userDto.Email) : null;
        }
    }
}