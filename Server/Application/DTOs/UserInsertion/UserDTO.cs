using API.Domain.Entities.UserManagement;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace API.Application.DTOs.UserInsertion;

public class UserDto
{
    [Required]
    [EmailAddress]
    public string email { get; set; } = string.Empty;
    [Required]
    public string password { get; set; } = string.Empty;
    [Required]
    public string name { get; set; } = string.Empty;
    [Required]
    [EnumDataType(typeof(UserType))]
    public UserType userType { get; set; }
}
