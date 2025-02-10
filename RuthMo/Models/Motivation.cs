using System.ComponentModel.DataAnnotations;

namespace RuthMo.Models;

public class Motivation
{
    [Key] public int Id { get; set; }
    [Required] [MaxLength(100)] public string Content { get; set; } = null!;
    public Author Author { get; set; } = null!;
}