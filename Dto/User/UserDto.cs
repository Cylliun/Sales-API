using SalesApi.Models;

namespace SalesApi.Dto.User;

public class UserDto
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public UserRole Role { get; set; }

}
