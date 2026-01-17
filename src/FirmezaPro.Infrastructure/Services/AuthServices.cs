using AuthIdentity.Infrastructure.Identity;
using FirmezaPro.Application.Dtos.Auth;
using FirmezaPro.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;



namespace FirmezaPro.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AuthResultDto> RegisterAsync(RegisterDto registerDto)
    {
        var user = new ApplicationUser
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            return new AuthResultDto
            {
                Success = false,
                Message = "Error al crear el usuario",
                Errors = result.Errors.Select(e => $"{e.Code}|{e.Description}").ToList()
            };
        }

        // ✅ Asignar rol por defecto al registrar
        await _userManager.AddToRoleAsync(user, "Customer");

        return new AuthResultDto
        {
            Success = true,
            Message = "Usuario registrado exitosamente",
            User = MapToUserDto(user)
        };
    }

    public async Task<AuthResultDto> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return new AuthResultDto { Success = false, Message = "Usuario o contraseña incorrectos" };

        var result = await _signInManager.PasswordSignInAsync(user, dto.Password, dto.RememberMe, false);
        if (!result.Succeeded)
            return new AuthResultDto { Success = false, Message = "Usuario o contraseña incorrectos" };

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Customer"; // fallback

        return new AuthResultDto
        {
            Success = true,
            Message = "Login exitoso",
            Role = role,
            User = MapToUserDto(user)
        };
    }

    public async Task<AuthResultDto> LogoutAsync()
    {
        await _signInManager.SignOutAsync();
        return new AuthResultDto { Success = true, Message = "Sesión cerrada correctamente" };
    }

   public async Task<AuthResultDto> GetCurrentUserAsync()
{
        // Obtener el Id del usuario actual
        var userId = _httpContextAccessor.HttpContext?.User 
            ?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return new AuthResultDto
            {
                Success = false,
                Message = "Usuario no autenticado"
            };
        }

        // Buscar el usuario en la base de datos
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new AuthResultDto
            {
                Success = false,
                Message = "Usuario no encontrado"
            };
        }

        // Obtener roles del usuario
        var roles = await _userManager.GetRolesAsync(user);

        return new AuthResultDto
        {
            Success = true,
            Message = "Usuario encontrado",
            User = MapToUserDto(user),
            Role = roles.FirstOrDefault() ?? "Customer" // fallback si no tiene rol
        };
    }

    public async Task<AuthResultDto> DeleteCurrentUserAsync(string password)
    {
        // Obtener el Id del usuario actual
        var userId = _httpContextAccessor.HttpContext?.User
            ?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return new AuthResultDto
            {
                Success = false,
                Message = "Usuario no autenticado"
            };
        }


        // Buscar el usuario
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new AuthResultDto
            {
                Success = false,
                Message = "Usuario no encontrado"
            };
        }

        // Verificar la contraseña
        var passwordValid = await _userManager.CheckPasswordAsync(user, password);
        if (!passwordValid)
        {
            return new AuthResultDto
            {
                Success = false,
                Message = "Contraseña incorrecta"
            };
        }

        // Cerrar sesión antes de eliminar
        await _signInManager.SignOutAsync();

        // Eliminar usuario
        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            return new AuthResultDto
            {
                Success = false,
                Message = "Error al eliminar el usuario",
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        return new AuthResultDto
        {
            Success = true,
            Message = "Cuenta eliminada correctamente"
        };
    }


    private UserDto MapToUserDto(ApplicationUser user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email!,
            UserName = user.UserName!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        };
    }
}
