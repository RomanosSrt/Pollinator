using API.Domain.Entities.UserManagement;

namespace API.Application.Services.Authentication
{
    public interface IJwtService
    {
        public string GenerateToken(ApplicationUser user);
    }
}
