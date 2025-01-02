using ProductsCRUD.Application.DTOs.Input;
using ProductsCRUD.Application.DTOs.Output;

namespace ProductsCRUD.Application.S_ProductService.Write
{
    public interface IProductWriteService
    {
        Task<AdminServiceLayerOutput<bool>> Create(ProductInput productInput);
        Task<AdminServiceLayerOutput<bool>> Update(ProductInput productInput);
        Task<AdminServiceLayerOutput<bool>> Delete(int productId);
        Task<AdminServiceLayerOutput<bool>> AssignPromotion(AssignPromotionInput assignPromotionInput);
    }
}
