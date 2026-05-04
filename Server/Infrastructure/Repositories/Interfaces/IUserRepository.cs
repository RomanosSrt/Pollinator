using API.Application.DTOs.UserInsertion;
using API.Domain.Entities.UserManagement;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<ApplicationUser?> GetUserByEmailAsync(string email);
        public Task<ApplicationUser?> GetUserByUsernameAsync(string username);
        public Task<bool> UserExistsByEmailAsync(string email);

    }
}
