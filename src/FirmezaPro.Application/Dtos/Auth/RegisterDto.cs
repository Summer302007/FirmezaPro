using System.ComponentModel.DataAnnotations;

namespace FirmezaPro.Application.Dtos.Auth;

public class RegisterDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(50, ErrorMessage = "El {0} no puede exceder los {1} caracteres")]
    [Display(Name = "Nombre")]
    public string? FirstName { get; set; }
        
    [Required(ErrorMessage = "El apellido es obligatorio")]
    [StringLength(50, ErrorMessage = "El {0} no puede exceder los {1} caracteres")]
    [Display(Name = "Apellido")]
    public string? LastName { get; set; }
        
    [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
    [Display(Name = "Usuario")]
    public string UserName { get; set; } = string.Empty;
        
    [Required(ErrorMessage = "El correo electrónico es obligatorio")]
    [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
    [Display(Name = "Correo Electrónico")]
    public string Email { get; set; } = string.Empty;
        
    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "La {0} debe tener entre {2} y {1} caracteres")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; } = string.Empty;
        
    [Required(ErrorMessage = "La confirmación es obligatoria")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirmar contraseña")]
    [Compare("Password", ErrorMessage = "La contraseña y la confirmación no coinciden")]
    public string ConfirmPassword { get; set; } = string.Empty;
}