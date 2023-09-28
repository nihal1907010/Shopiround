using Microsoft.EntityFrameworkCore;
using Shopiround.Models.Models;

namespace Shopiround.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        DbSet<Shop> Shops { get; set; }
        DbSet<Product> Products { get; set; }
    }
}
