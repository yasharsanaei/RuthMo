using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RuthMo.Models;

public class Motivation : _BaseModel
{
    [MaxLength(4000)] public required string Content { get; set; } = string.Empty;
    public required MotivationStatus Status { get; set; } = MotivationStatus.Waiting;
    public required string UserId { get; set; }
    [ForeignKey(nameof(UserId))] public virtual RuthMoUser User { get; set; }
}