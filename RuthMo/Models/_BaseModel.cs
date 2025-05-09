using System.ComponentModel.DataAnnotations;

namespace RuthMo.Models;

public class _BaseModel
{
    [Key] public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}