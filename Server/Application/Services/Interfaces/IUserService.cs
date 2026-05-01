using API.Application.DTOs.UserInsertion;
using API.Domain.Entities.UserManagement;


namespace API.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<AuthResultDto> CreateUser(UserDto user);
        Task<string?> LoginUser(LoginDto loginDto);
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task<UserDto?> GetUserByUsernameAsync(string username);
    }
}