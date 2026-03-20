using API.Application.DTOs.UserInsertion;
using API.Application.Services.Interfaces;
using API.Domain.Entities.UserManagement;
using API.Infrastructure.Repositories.Interfaces;

namespace API.Application.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public User CreateUser(UserDto user)
        {
            var newUser = new User
            {
                id = Guid.NewGuid(),
                name = user.name,
                email = user.email,
                usertype = user.userType
            };
            return _userRepository.Add(newUser);
        }

        public void DeleteUser(Guid userId)
        {
            var user = this.GetUser(userId);
        }

        public User UpdateUser(Guid userId, UserDto user)
        {
            throw new NotImplementedException();
        }

        public User? GetUser(Guid userId)
        {
            return _userRepository.Get(userId);
        }
    }
}
