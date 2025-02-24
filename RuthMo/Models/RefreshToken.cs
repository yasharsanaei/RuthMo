using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RuthMo.Models;

public class RefreshToken
{
    [Key] public int Id { get; set; }
    public required string Token { get; set; }
    public required string JwtId { get; set; }
    public required bool IsRevoked { get; set; }
    public required DateTime DateAdded { get; set; }
    public required DateTime DateExpire { get; set; }
    public string UserId { get; set; }
    [ForeignKey(nameof(UserId))] public virtual RuthMoUser User { get; set; }
}