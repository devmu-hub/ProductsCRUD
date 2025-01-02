using ProductsCRUD.Application.DTOs.Input;
using ProductsCRUD.Application.DTOs.Output;
using AutoMapper;
using ProductsCRUD.Application.DTOs.Validators;
using ProductsCRUD.Domain._core;
using ProductsCRUD.Domain.Products;

namespace ProductsCRUD.Application.S_ProductService.Write
{
    public class ProductWriteService(IMapper mapper,
        IUnitOfWork unitOfWork) : IProductWriteService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;


        public async Task<AdminServiceLayerOutput<bool>> Create(ProductInput productInput)
        {
            try
            {
                ProductInputValidator validator = new();

                var validationResult = await validator.ValidateAsync(productInput);

                if (!validationResult.IsValid)
                    return new AdminServiceLayerOutput<bool>
                    {
                        ErrorMessages = validationResult.Errors.Select(e => e.ErrorMessage).Distinct().ToList()
                    };

                Product product = _mapper.Map<Product>(productInput);

                await _unitOfWork.ProductRepository.Add(product);

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

        public async Task<AdminServiceLayerOutput<bool>> Update(ProductInput productInput)
        {
            try
            {
                ProductInputValidator validator = new();

                var validationResult = await validator.ValidateAsync(productInput);

                if (!validationResult.IsValid)
                    return new AdminServiceLayerOutput<bool>
                    {
                        ErrorMessages = validationResult.Errors.Select(e => e.ErrorMessage).Distinct().ToList()
                    };

                Product product = await _unitOfWork.ProductRepository.Get(p => p.Id == productInput.Id);

                if (product is null)
                    return new AdminServiceLayerOutput<bool>
                    {
                        ErrorMessages = ["Product id is not exist"]
                    };

                product.Name = productInput.Name;
                product.Description = productInput.Description;
                product.Price = productInput.Price;

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

        public async Task<AdminServiceLayerOutput<bool>> Delete(int productId)
        {
            try
            {
                if (productId <= 0)
                    return new AdminServiceLayerOutput<bool>
                    {
                        ErrorMessages = ["Invalid product id"]
                    };

                Product product = await _unitOfWork.ProductRepository.Get(p => p.Id == productId);

                if (product is null)
                    return new AdminServiceLayerOutput<bool>
                    {
                        ErrorMessages = ["Product id is not exist"]
                    };

                _unitOfWork.ProductRepository.HardRemove(product);

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

        public async Task<AdminServiceLayerOutput<bool>> AssignPromotion(AssignPromotionInput assignPromotionInput)
        {
            try
            {
                if (assignPromotionInput is null || assignPromotionInput.ProductId <= 0 || assignPromotionInput.PromotionId <= 0)
                    return new AdminServiceLayerOutput<bool>
                    {
                        ErrorMessages = ["Invalid product/promotion id"]
                    };

                if (!await _unitOfWork.ProductRepository.Any(p => p.Id == assignPromotionInput.ProductId))
                    return new AdminServiceLayerOutput<bool>
                    {
                        ErrorMessages = ["Product id is not exist"]
                    };

                if (!await _unitOfWork.PromotionRepository.Any(p => p.Id == assignPromotionInput.PromotionId))
                    return new AdminServiceLayerOutput<bool>
                    {
                        ErrorMessages = ["Promotion id is not exist"]
                    };

                await _unitOfWork.ProductPromotionRepository.Add(new ProductPromotion
                {
                    ProductId = assignPromotionInput.ProductId,
                    PromotionId = assignPromotionInput.PromotionId
                });

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
