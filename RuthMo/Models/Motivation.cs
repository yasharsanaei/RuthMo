using System.Text.Json.Serialization;

namespace RuthMo.Models;

public class Motivation
{
    public required int Id { get; set; }
    public required string Content { get; set; } = string.Empty;
    public required int AuthorId { get; set; }

    [JsonIgnore] public virtual Author Author { get; set; }
}