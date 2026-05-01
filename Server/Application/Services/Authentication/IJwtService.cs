using API.Application.DTOs.UserInsertion;
using API.Domain.Entities.UserManagement;

namespace API.Application.Services.Authentication
{
    public interface IJwtService
    {
        public AuthResultDto GenerateToken(ApplicationUser user);
    }
}
