using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shopiround.Models.Models;

namespace Shopiround.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        DbSet<Shop> Shops { get; set; }
        DbSet<Product> Products { get; set; }

        DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
    }
}
