using System.ComponentModel.DataAnnotations;

namespace RuthMo.Models;

public class Author
{
    [Key] public int Id { get; set; }
    [Required] [MaxLength(50)] public string Name { get; set; } = null!;
    public string? NickName { get; set; }
    public ICollection<Motivation> Motivations { get; set; } = null!;
}