using AuthIdentity.Infrastructure.Identity;
using AuthIdentity.Infrastructure.Services;
using FirmezaPro.Application.Interfaces;
using FirmezaPro.Application.Interfaces.Auth;
using FirmezaPro.Application.Services;
using FirmezaPro.Infrastructure.Persistence;
using FirnezaPro.Domain.Interfaces;
using FirnezaPro.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------
// Add services to the container
// ---------------------------
builder.Services.AddControllersWithViews();

// DbContext con PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// AuthService para login/register
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthService, AuthService>();

// Registrar IProductRepository y su implementación concreta ProductRepository
builder.Services.AddScoped<IProductRepository, ProductRepository>(); // Registra IProductRepository
builder.Services.AddScoped<IProductService, ProductService>();  // Registra IProductService

// Configuración de cookies para login
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";              // Login normal
    options.AccessDeniedPath = "/Account/AccessDenied"; // Página dedicada
});

var app = builder.Build();

// ---------------------------
// Crear roles y usuario Admin inicial
// ---------------------------
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roles = new[] { "Admin", "Customer" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Crear usuario Admin inicial si no existe
    string adminEmail = "admin@firmeza.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var admin = new ApplicationUser
        {
            UserName = "admin",
            Email = adminEmail,
            FirstName = "Super",
            LastName = "Admin",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await userManager.CreateAsync(admin, "Admin123!"); // Contraseña inicial
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}

// ---------------------------
// Configure the HTTP request pipeline
// ---------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
