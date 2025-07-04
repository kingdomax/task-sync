using TaskSync.Models;
using TaskSync.Repositories.Entities;
using TaskSync.Repositories.Interfaces;
using TaskSync.Services.Interfaces;

namespace TaskSync.Services
{
    // todo-moch: remove this service and User model
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
