using ProductsCRUD.Domain._core;

namespace ProductsCRUD.Domain.Products
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsForMobileAsync(bool? isFeatured, bool? isNew, string search, int skip, int take);
        Task<int> CountProductsForMobileAsync(bool? isFeatured, bool? isNew, string search);
    }
}
