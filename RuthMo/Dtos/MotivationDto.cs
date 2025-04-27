using RuthMo.Models;

namespace RuthMo.Dtos;

public class MotivationDto : BaseDto
{
    public required string Content { get; set; }
    public required MotivationStatus Status { get; set; }

    public required UserDto User { get; set; }
}