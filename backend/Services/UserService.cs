using TaskSync.Services.Interfaces;
using TaskSync.Models;
using TaskSync.Repositories.Interfaces;
using TaskSync.Repositories.Entities;

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
            var userEntity = await _userRepository.GetAsync();

            return userEntity != null 
                    ? new User(userEntity.Id, userEntity.Username, userEntity.Firstname, userEntity.Lastname, userEntity.Email, userEntity.Password)
                    : null;
        }

        public async Task<User?> GetUserAsync(string email)
        {
            var userEntity = await _userRepository.GetAsync(email);

            return userEntity != null
                    ? new User(userEntity.Id, userEntity.Username, userEntity.Firstname, userEntity.Lastname, userEntity.Email, userEntity.Password)
                    : null;
        }
    }
}