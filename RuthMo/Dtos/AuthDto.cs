namespace RuthMo.Dtos;

public class AuthDto
{
    public required string Token { get; set; }
    public required string RefreshToken { get; set; }
    public required DateTime ExpiresAt { get; set; }
}