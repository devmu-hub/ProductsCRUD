using ProductsCRUD.Application.DTOs.Input;
using ProductsCRUD.Application.DTOs.Output;

namespace ProductsCRUD.Application.S_ProductService.Read
{
    public interface IProductReadService
    {
        Task<AdminServiceLayerListOutput<ProductOutput>> GetAll();
        Task<AdminServiceLayerOutput<ProductOutput>> Get(int productId);
        Task<AdminServiceLayerListOutput<MobileProductOutput>> GetProductsForMobile(MobileProductSearchInput mobileProductSearchInput);
    }
}
