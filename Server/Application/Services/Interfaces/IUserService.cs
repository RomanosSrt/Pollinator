using API.Application.DTOs.UserInsertion;
using API.Domain.Entities.UserManagement;


namespace API.Application.Services.Interfaces
{
    public interface IUserService
    {
        User CreateUser(UserDTO user);

        User UpdateUser(Guid userId, UserDTO user);

        void DeleteUser(Guid userId);

        User? GetUser(Guid userId);
    }
}
