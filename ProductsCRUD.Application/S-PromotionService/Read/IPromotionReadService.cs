using ProductsCRUD.Application.DTOs.Output;

namespace ProductsCRUD.Application.S_PromotionService.Read
{
    public interface IPromotionReadService
    {
        Task<AdminServiceLayerListOutput<PromotionOutput>> GetAll();
        Task<AdminServiceLayerOutput<PromotionOutput>> Get(int promotionId);
    }
}
