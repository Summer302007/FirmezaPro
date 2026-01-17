using Microsoft.EntityFrameworkCore;
using FirmezaPro.Domain.Entities;

namespace FirmezaPro.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products => Set<Product>();
    }
}