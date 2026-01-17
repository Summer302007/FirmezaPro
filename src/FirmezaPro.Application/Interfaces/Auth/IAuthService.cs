using FirmezaPro.Application.Dtos.Auth;

namespace FirmezaPro.Application.Interfaces.Auth;

public interface IAuthService
{
    Task<AuthResultDto> RegisterAsync(RegisterDto registerDto);
    Task<AuthResultDto> LoginAsync(LoginDto loginDto);
    Task<AuthResultDto> LogoutAsync();
    Task<AuthResultDto> GetCurrentUserAsync();
    Task<AuthResultDto> DeleteCurrentUserAsync(string password);
}
