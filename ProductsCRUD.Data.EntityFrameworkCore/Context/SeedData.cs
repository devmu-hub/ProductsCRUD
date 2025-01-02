using Microsoft.EntityFrameworkCore;
using ProductsCRUD.Domain.AdminUsers;
using ProductsCRUD.Domain.Promotions;

namespace ProductsCRUD.Data.EntityFrameworkCore.Context
{
    public static class SeedData
    {
        public static void DefaultAdminUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminUser>().HasData(
            [
                new()
                {
                    Id = 1,
                    UserName = "Admin",
                    Password = "1N5IJ8IQP4VDV3RiEBAWWQ==", // Admin
                    IsActive = true
                }
            ]);
        }

        public static void DefaultPromotionTypes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PromotionType>().HasData(
            [
                new() { Id = 1, Name = "Discount" },
                new() { Id = 2, Name = "BuyOneGetOne" }
            ]);
        }




    }
}
