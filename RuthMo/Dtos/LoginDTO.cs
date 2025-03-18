using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RuthMo.Dtos;

public class LoginDto
{
    [EmailAddress] public required string Email { get; set; }
    [PasswordPropertyText] public required string Password { get; set; }
}