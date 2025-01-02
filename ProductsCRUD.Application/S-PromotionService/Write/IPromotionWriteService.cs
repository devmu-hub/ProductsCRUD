using ProductsCRUD.Application.DTOs.Input;
using ProductsCRUD.Application.DTOs.Output;

namespace ProductsCRUD.Application.S_PromotionService.Write
{
    public interface IPromotionWriteService
    {
        Task<AdminServiceLayerOutput<bool>> Create(PromotionInput promotionInput);
        Task<AdminServiceLayerOutput<bool>> Update(PromotionInput promotionInput);
        Task<AdminServiceLayerOutput<bool>> Delete(int promotionId);
    }
}
