using ProductsCRUD.Domain._core;

namespace ProductsCRUD.Domain.Promotions
{
    public interface IPromotionRepository : IRepository<Promotion>
    {
        Task<IEnumerable<Promotion>> GetPromotionsAsync();
        Task<Promotion> GetPromotionByIdAsync(int promotionId);
    }
}
