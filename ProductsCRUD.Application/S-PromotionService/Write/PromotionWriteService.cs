using ProductsCRUD.Application.DTOs.Input;
using ProductsCRUD.Application.DTOs.Output;
using AutoMapper;
using ProductsCRUD.Application.DTOs.Validators;
using ProductsCRUD.Domain._core;
using ProductsCRUD.Domain.Promotions;

namespace ProductsCRUD.Application.S_PromotionService.Write
{
    public class PromotionWriteService(IMapper mapper,
        IUnitOfWork unitOfWork) : IPromotionWriteService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;



        public async Task<AdminServiceLayerOutput<bool>> Create(PromotionInput promotionInput)
        {
            try
            {
                PromotionInputValidator validator = new();

                var validationResult = await validator.ValidateAsync(promotionInput);

                if (!validationResult.IsValid)
                    return new AdminServiceLayerOutput<bool>
                    {
                        ErrorMessages = validationResult.Errors.Select(e => e.ErrorMessage).Distinct().ToList()
                    };

                Promotion promotion = _mapper.Map<Promotion>(promotionInput);

                await _unitOfWork.PromotionRepository.Add(promotion);

                await _unitOfWork.Complete();

                return new AdminServiceLayerOutput<bool>
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new AdminServiceLayerOutput<bool>
                {
                    IsExistException = true,
                    ErrorMessages = [ex.Message]
                };
            }
        }

        public async Task<AdminServiceLayerOutput<bool>> Update(PromotionInput promotionInput)
        {
            try
            {
                PromotionInputValidator validator = new();

                var validationResult = await validator.ValidateAsync(promotionInput);

                if (!validationResult.IsValid)
                    return new AdminServiceLayerOutput<bool>
                    {
                        ErrorMessages = validationResult.Errors.Select(e => e.ErrorMessage).Distinct().ToList()
                    };

                Promotion promotion = await _unitOfWork.PromotionRepository.Get(p => p.Id == promotionInput.Id);

                if (promotion is null)
                    return new AdminServiceLayerOutput<bool>
                    {
                        ErrorMessages = ["Promotion id is not exist"]
                    };

                promotion.PromotionTypeId = promotionInput.PromotionTypeId;
                promotion.Name = promotionInput.Name;
                promotion.DiscountPercentage = promotionInput.DiscountPercentage;

                await _unitOfWork.Complete();

                return new AdminServiceLayerOutput<bool>
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new AdminServiceLayerOutput<bool>
                {
                    IsExistException = true,
                    ErrorMessages = [ex.Message]
                };
            }
        }

        public async Task<AdminServiceLayerOutput<bool>> Delete(int promotionId)
        {
            try
            {
                if (promotionId <= 0)
                    return new AdminServiceLayerOutput<bool>
                    {
                        ErrorMessages = ["Invalid product id"]
                    };

                Promotion promotion = await _unitOfWork.PromotionRepository.Get(p => p.Id == promotionId);

                if (promotion is null)
                    return new AdminServiceLayerOutput<bool>
                    {
                        ErrorMessages = ["Promotion id is not exist"]
                    };

                _unitOfWork.PromotionRepository.HardRemove(promotion);

                await _unitOfWork.Complete();

                return new AdminServiceLayerOutput<bool>
                {
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                return new AdminServiceLayerOutput<bool>
                {
                    IsExistException = true,
                    ErrorMessages = [ex.Message]
                };
            }
        }



    }
}
