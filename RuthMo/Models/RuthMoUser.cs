using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace RuthMo.Models;

public class RuthMoUser : IdentityUser
{
    [MaxLength(50)] [Required] public required string FirstName { get; set; } = string.Empty;
    [MaxLength(50)] [Required] public required string LastName { get; set; } = string.Empty;
    public required DateTime CreatedAt { get; set; }
}