using Microsoft.AspNetCore.Identity;

namespace API.Domain.Entities.UserManagement
{
    public class ApplicationUser : IdentityUser
    {
        public string fullName { get; set; } = string.Empty;
        public UserType userType { get; set; }
    }
}
