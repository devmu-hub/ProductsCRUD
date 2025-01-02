using ProductsCRUD.Domain.AdminUsers;
using ProductsCRUD.Domain.Products;
using ProductsCRUD.Domain.Promotions;

namespace ProductsCRUD.Domain._core
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<AdminUser> AdminUserRepository { get; }
        IRepository<ProductPromotion> ProductPromotionRepository { get; }
        IRepository<PromotionType> PromotionTypeRepository { get; }


        IPromotionRepository PromotionRepository { get; }
        IProductRepository ProductRepository { get; }




        Task Complete();
    }
}
