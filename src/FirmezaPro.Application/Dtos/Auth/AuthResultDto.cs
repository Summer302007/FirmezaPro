namespace FirmezaPro.Application.Dtos.Auth;

public class AuthResultDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public UserDto? User { get; set; }
    public List<string> Errors { get; set; } = new();
    public string? Role { get; set; } // Admin o Customer
}