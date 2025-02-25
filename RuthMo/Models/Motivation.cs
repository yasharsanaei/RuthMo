using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RuthMo.Models;

public class Motivation
{
    [Key] public int Id { get; set; }

    [MaxLength(100)] public required string Content { get; set; } = string.Empty;
    public required int UserId { get; set; }
    [ForeignKey(nameof(UserId))] public virtual required RuthMoUser User { get; set; }
}