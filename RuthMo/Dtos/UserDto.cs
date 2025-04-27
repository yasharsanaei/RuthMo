using RuthMo.Models;

namespace RuthMo.Dtos;

public class UserDto : BaseDto
{
    public required string FirstName { get; set; } = string.Empty;
    public required string LastName { get; set; } = string.Empty;
    public required string UserName { get; set; } = string.Empty;
    public required string Email { get; set; } = string.Empty;
    public required List<string> Roles { get; set; } = [];
}