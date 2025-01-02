using Microsoft.EntityFrameworkCore;
using ProductsCRUD.Data.EntityFrameworkCore.Context;
using ProductsCRUD.Data.EntityFrameworkCore.Repositories._core;
using ProductsCRUD.Domain.Promotions;

namespace ProductsCRUD.Data.EntityFrameworkCore.Repositories.Promotions
{
    public class PromotionRepository(ApplicationDbContext context) : Repository<Promotion>(context), IPromotionRepository
    {
        private readonly ApplicationDbContext _context = context;


        public async Task<IEnumerable<Promotion>> GetPromotionsAsync()
        {
            return await _context.Promotions
                .Select(p => new Promotion
                {
                    Id = p.Id,
                    Name = p.Name,
                    PromotionTypeId = p.PromotionTypeId,
                    DiscountPercentage = p.DiscountPercentage,
                    PromotionType = new PromotionType
                    {
                        Name = p.PromotionType.Name
                    }
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Promotion> GetPromotionByIdAsync(int promotionId)
        {
            return await _context.Promotions
                .Where(p => p.Id == promotionId)
                .Select(p => new Promotion
                {
                    Id = p.Id,
                    Name = p.Name,
                    PromotionTypeId = p.PromotionTypeId,
                    DiscountPercentage = p.DiscountPercentage,
                    PromotionType = new PromotionType
                    {
                        Name = p.PromotionType.Name
                    }
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }


    }
}
