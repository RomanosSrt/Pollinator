using API.Domain.Entities.UserManagement;

namespace API.Application.DTOs.UserInsertion;

public class UserDTO
{
    public Guid id { get; set; }
    public string name { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public UserType userType { get; set; }
}
