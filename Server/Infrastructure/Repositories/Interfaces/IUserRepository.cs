using API.Application.DTOs.UserInsertion;
using API.Domain.Entities.UserManagement;

namespace API.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetUserByEmailAsync(string email);
        Task<ApplicationUser?> GetUserByUsernameAsync(string username);
    }
}
