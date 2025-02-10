namespace RuthMo.Models;

public class Motivation
{
    public required int Id { get; set; }
    public required string Content { get; set; } = string.Empty;
    public required string Author { get; set; } = string.Empty;
}