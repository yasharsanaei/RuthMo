namespace RuthMo.Models;

public class Author
{
    public required int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string NickName { get; set; }
    public string? Email { get; set; }

    public virtual required List<Motivation> Motivations { get; set; }
}