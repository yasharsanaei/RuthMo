using RuthMo.Models;

namespace RuthMo.Dtos;

public class MotivationDto
{
    public required int Id { get; set; }
    public required string Content { get; set; }
    public required MotivationStatus Status { get; set; }

    public required UserDTO User { get; set; }
}