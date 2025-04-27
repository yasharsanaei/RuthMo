namespace RuthMo.Dtos;

public class UserUpdateDto : BaseUpdateDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string? Email { get; set; }
}