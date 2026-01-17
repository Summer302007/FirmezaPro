using System.ComponentModel.DataAnnotations;

namespace FirmezaPro.Application.Dtos.Auth;

public class LoginDto
{
    [Required(ErrorMessage = "El correo electrónico es obligatorio")]
    [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
    [Display(Name = "Correo Electrónico")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; } = string.Empty;
    
    [Display(Name = "Recordarme")]
    public bool RememberMe { get; set; }
}