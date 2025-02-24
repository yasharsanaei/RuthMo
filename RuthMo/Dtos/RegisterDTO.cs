using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RuthMo.Dtos;

public class RegisterDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    [Required] [EmailAddress] public required string Email { get; set; }
    [Required] [PasswordPropertyText] public required string Password { get; set; }
}