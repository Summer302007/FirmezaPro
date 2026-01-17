using FirmezaPro.Application.Dtos.Auth;
using FirmezaPro.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Mvc;

namespace FirmezaPro.Web.Views.Account;

public class AccountController : Controller
{
    private readonly IAuthService _authService;

    public AccountController(IAuthService authService)
    {
        _authService = authService;
    }
    
    
    // GET
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _authService.RegisterAsync(model);

        if (!result.Success)
        {
            foreach (var error in result.Errors)
            {
                var parts = error.Split('|');
                var code = parts[0];
                var message = parts.Length > 1 ? parts[1] : error;

                if (code == "DuplicateEmail")
                {
                    ModelState.AddModelError(
                        nameof(RegisterDto.Email),
                        message
                    );
                }
                else if (code == "DuplicateUserName")
                {
                    ModelState.AddModelError(
                        nameof(RegisterDto.UserName),
                        message
                    );
                }
                else
                {
                    ModelState.AddModelError(string.Empty, message);
                }
            }

            return View(model);
        }

        TempData["SuccessMessage"] = result.Message;
        return RedirectToAction("Index", "Home");
    }
    
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _authService.LoginAsync(model);

        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }

        // Aquí comprobamos el rol devuelto por el AuthService
        if (result.Role == "Admin")
            return RedirectToAction("Index", "Product");
        else if (result.Role == "Customer")
            return RedirectToAction("Index", "Product");
    
        // fallback por si algo falla
        return RedirectToAction("Index", "Home");
    }
    
    [HttpGet]
    public IActionResult AccessDenied(string returnUrl)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return RedirectToAction("Login");
    }
}