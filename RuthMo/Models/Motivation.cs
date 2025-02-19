using System.ComponentModel.DataAnnotations;

namespace RuthMo.Models;

public class Motivation
{
    [Key] public int Id { get; set; }
    [Required] [MaxLength(140)] public string Content { get; set; } = String.Empty;

    public virtual User User { get; set; } = new User();
}