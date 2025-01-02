using AutoMapper;
using ProductsCRUD.Application.DTOs.Output;
using ProductsCRUD.Domain._core;
using ProductsCRUD.Domain.Promotions;

namespace ProductsCRUD.Application.S_PromotionTypeService.Read
{
    public class PromotionTypeReadService(IUnitOfWork unitOfWork,
        IMapper mapper) : IPromotionTypeReadService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;


        public async Task<AdminServiceLayerListOutput<PromotionTypeOutput>> GetAll()
        {
            try
            {
                IEnumerable<PromotionType> promotionTypes = await _unitOfWork.PromotionTypeRepository.GetAll();

                return new AdminServiceLayerListOutput<PromotionTypeOutput>
                {
                    Success = true,
                    Data = _mapper.Map<IEnumerable<PromotionTypeOutput>>(promotionTypes)
                };
            }
            catch (Exception ex)
            {
                return new AdminServiceLayerListOutput<PromotionTypeOutput>
                {
                    IsExistException = true,
                    ErrorMessages = [ex.Message]
                };
            }
        }


    }
}
