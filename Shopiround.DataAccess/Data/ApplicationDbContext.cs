using Microsoft.EntityFrameworkCore;
using Shopiround.Models.Models;

namespace Shopiround.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Shop> Shops { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
