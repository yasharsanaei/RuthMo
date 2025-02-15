using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace RuthMo.Models;

public class User : IdentityUser
{
    [EmailAddress] public override string Email { get; set; } = "";
    [MaxLength(50)] public string? NickName { get; set; }
    public Gender? Gender { get; set; } = null;
    public ICollection<Motivation> Motivations { get; set; } = null!;
}