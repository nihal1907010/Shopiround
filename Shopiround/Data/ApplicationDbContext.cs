using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shopiround.Models;
using Shopiround.Models.Statistics;

namespace Shopiround.Data
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
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<SavedItem> SavedItems { get; set; }
        public DbSet<KeywordsCount> KeywordsCounts { get; set; }
        public DbSet<ProductCount> ProductCounts { get; set; }        
        public DbSet<DiscountDate> DiscountDates { get; set; }
    }
}
