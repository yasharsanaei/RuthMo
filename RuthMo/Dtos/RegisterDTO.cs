using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RuthMo.Dtos;

public class RegisterDto
{
    public required string FirstName { get; set; } = string.Empty;
    public required string LastName { get; set; } = string.Empty;
    [EmailAddress] public required string Email { get; set; }
    public required string Username { get; set; }
    [PasswordPropertyText] public required string Password { get; set; }
}