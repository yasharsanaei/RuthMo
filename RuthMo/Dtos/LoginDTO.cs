using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RuthMo.Dtos;

public class LoginDto
{
    [Required] [EmailAddress] public required string Email { get; set; }
    [Required] [PasswordPropertyText] public required string Password { get; set; }
}