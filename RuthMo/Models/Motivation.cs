using System.ComponentModel.DataAnnotations;

namespace RuthMo.Models;

public class Motivation
{
    [Key] public int Id { get; set; }
    [Required] public string Content { get; set; } = String.Empty;

    public virtual User User { get; }
}