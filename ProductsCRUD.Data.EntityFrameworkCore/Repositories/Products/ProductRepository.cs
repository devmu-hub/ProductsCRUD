using Microsoft.EntityFrameworkCore;
using ProductsCRUD.Data.EntityFrameworkCore.Context;
using ProductsCRUD.Data.EntityFrameworkCore.Repositories._core;
using ProductsCRUD.Domain.Products;

namespace ProductsCRUD.Data.EntityFrameworkCore.Repositories.Products
{
    public class ProductRepository(ApplicationDbContext context) : Repository<Product>(context), IProductRepository
    {
        private readonly ApplicationDbContext _context = context;


        public async Task<IEnumerable<Product>> GetProductsForMobileAsync(bool? isFeatured, bool? isNew, string search, int skip, int take)
        {
            return await _context.Products
                .Where(p =>
                    (!isFeatured.HasValue || p.IsFeatured == isFeatured) &&
                    (!isNew.HasValue || (isNew.Value && p.CreatedAt >= DateTime.Now.AddDays(-15))) &&
                    (string.IsNullOrEmpty(search) || p.Name.ToLower().Equals(search.ToLower()) || p.Description.ToLower().Equals(search.ToLower()))
                )
                .Select(p => new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ProductPromotions = p.ProductPromotions.Select(pp => new ProductPromotion
                    {
                        Promotion = new Domain.Promotions.Promotion
                        {
                            Name = pp.Promotion.Name,
                            DiscountPercentage = pp.Promotion.DiscountPercentage
                        }
                    }).ToList()
                })
                .AsNoTracking()
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<int> CountProductsForMobileAsync(bool? isFeatured, bool? isNew, string search)
        {
            return await _context.Products
                .Where(p =>
                    (isFeatured == null || p.IsFeatured == isFeatured) &&
                    (!isNew.HasValue || (isNew.Value && p.CreatedAt >= DateTime.Now.AddDays(-15))) &&
                    (string.IsNullOrEmpty(search) || p.Name.ToLower().Equals(search.ToLower()) || p.Description.ToLower().Equals(search.ToLower()))
                ).CountAsync();
        }

    }
}
