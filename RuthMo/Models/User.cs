using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace RuthMo.Models;

public class User : IdentityUser
{
    [Required] public string Nickname { get; set; } = String.Empty;

    public virtual ICollection<Motivation> Motivations { get; set; } = new List<Motivation>([]);
}