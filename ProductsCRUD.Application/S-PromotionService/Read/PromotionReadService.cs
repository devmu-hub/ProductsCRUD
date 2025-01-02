using ProductsCRUD.Application.DTOs.Output;
using AutoMapper;
using ProductsCRUD.Domain._core;
using ProductsCRUD.Domain.Promotions;

namespace ProductsCRUD.Application.S_PromotionService.Read
{
    public class PromotionReadService(IUnitOfWork unitOfWork,
        IMapper mapper) : IPromotionReadService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;


        public async Task<AdminServiceLayerListOutput<PromotionOutput>> GetAll()
        {
            try
            {
                IEnumerable<Promotion> promotions = await _unitOfWork.PromotionRepository.GetPromotionsAsync();

                return new AdminServiceLayerListOutput<PromotionOutput>
                {
                    Success = true,
                    Data = _mapper.Map<IEnumerable<PromotionOutput>>(promotions)
                };
            }
            catch (Exception ex)
            {
                return new AdminServiceLayerListOutput<PromotionOutput>
                {
                    IsExistException = true,
                    ErrorMessages = [ex.Message]
                };
            }
        }

        public async Task<AdminServiceLayerOutput<PromotionOutput>> Get(int promotionId)
        {
            try
            {
                if (promotionId <= 0)
                    return new AdminServiceLayerOutput<PromotionOutput>
                    {
                        ErrorMessages = ["Invalid promotion id"]
                    };

                Promotion promotion = await _unitOfWork.PromotionRepository.GetPromotionByIdAsync(promotionId);

                if (promotion is null)
                    return new AdminServiceLayerOutput<PromotionOutput>
                    {
                        ErrorMessages = ["Promotion id is not exist"]
                    };

                return new AdminServiceLayerOutput<PromotionOutput>
                {
                    Success = true,
                    Data = _mapper.Map<PromotionOutput>(promotion)
                };
            }
            catch (Exception ex)
            {
                return new AdminServiceLayerOutput<PromotionOutput>
                {
                    IsExistException = true,
                    ErrorMessages = [ex.Message]
                };
            }
        }



    }
}
