using ProductsCRUD.Application.DTOs.Output;

namespace ProductsCRUD.Application.S_PromotionTypeService.Read
{
    public interface IPromotionTypeReadService
    {
        Task<AdminServiceLayerListOutput<PromotionTypeOutput>> GetAll();
    }
}
