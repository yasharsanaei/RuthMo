using RuthMo.Models;

namespace RuthMo.Dtos;

public class UserDTO
{
    public required string Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public List<string> Roles { get; set; } = [];
}