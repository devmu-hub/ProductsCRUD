using Microsoft.EntityFrameworkCore;
using ProductsCRUD.Domain.AdminUsers;
using ProductsCRUD.Domain.Products;
using ProductsCRUD.Domain.Promotions;

namespace ProductsCRUD.Data.EntityFrameworkCore.Context
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PromotionType> PromotionTypes { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<ProductPromotion> ProductPromotions { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductPromotion>().HasKey(pp => new { pp.ProductId, pp.PromotionId });

            SeedData.DefaultAdminUser(modelBuilder);

            SeedData.DefaultPromotionTypes(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }



    }
}
