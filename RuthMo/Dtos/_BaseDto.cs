namespace RuthMo.Dtos;

public class BaseDto
{
    public required string Id { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime ModifiedAt { get; set; }
}