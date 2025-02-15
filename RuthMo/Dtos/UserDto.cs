using RuthMo.Models;

namespace RuthMo.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string? NickName { get; set; }
    public Gender? Gender { get; set; }
    public ICollection<Motivation> Motivations { get; set; }
}