using ProductsCRUD.Application.DTOs.Input;
using ProductsCRUD.Application.DTOs.Output;
using AutoMapper;
using ProductsCRUD.Domain._core;
using ProductsCRUD.Domain.Products;

namespace ProductsCRUD.Application.S_ProductService.Read
{
    public class ProductReadService(IMapper mapper,
        IUnitOfWork unitOfWork) : IProductReadService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;


        public async Task<AdminServiceLayerListOutput<ProductOutput>> GetAll()
        {
            try
            {
                IEnumerable<Product> products = await _unitOfWork.ProductRepository.GetAll();

                return new AdminServiceLayerListOutput<ProductOutput>
                {
                    Success = true,
                    Data = _mapper.Map<IEnumerable<ProductOutput>>(products)
                };
            }
            catch (Exception ex)
            {
                return new AdminServiceLayerListOutput<ProductOutput>
                {
                    IsExistException = true,
                    ErrorMessages = [ex.Message]
                };
            }
        }

        public async Task<AdminServiceLayerOutput<ProductOutput>> Get(int productId)
        {
            try
            {
                if (productId <= 0)
                    return new AdminServiceLayerOutput<ProductOutput>
                    {
                        ErrorMessages = ["Invalid product id"]
                    };

                Product product = await _unitOfWork.ProductRepository.GetWithNoTracking(p => p.Id == productId);

                if (product is null)
                    return new AdminServiceLayerOutput<ProductOutput>
                    {
                        ErrorMessages = ["Product id is not exist"]
                    };

                return new AdminServiceLayerOutput<ProductOutput>
                {
                    Success = true,
                    Data = _mapper.Map<ProductOutput>(product)
                };
            }
            catch (Exception ex)
            {
                return new AdminServiceLayerOutput<ProductOutput>
                {
                    IsExistException = true,
                    ErrorMessages = [ex.Message]
                };
            }
        }

        public async Task<AdminServiceLayerListOutput<MobileProductOutput>> GetProductsForMobile(MobileProductSearchInput mobileProductSearchInput)
        {
            try
            {
                if (mobileProductSearchInput is null || mobileProductSearchInput.PageNumber <= 0 || mobileProductSearchInput.PageSize <= 0)
                    return new AdminServiceLayerListOutput<MobileProductOutput>
                    {
                        ErrorMessages = ["Invalid input"]
                    };

                int skip = (mobileProductSearchInput.PageNumber - 1) * mobileProductSearchInput.PageSize;

                IEnumerable<Product> products = await _unitOfWork.ProductRepository.GetProductsForMobileAsync(mobileProductSearchInput.IsFeatured,
                    mobileProductSearchInput.IsNew, mobileProductSearchInput.Search, skip, mobileProductSearchInput.PageSize);

                return new AdminServiceLayerListOutput<MobileProductOutput>
                {
                    Success = true,
                    Data = _mapper.Map<IEnumerable<MobileProductOutput>>(products),
                    Count = await _unitOfWork.ProductRepository.CountProductsForMobileAsync(mobileProductSearchInput.IsFeatured,
                        mobileProductSearchInput.IsNew, mobileProductSearchInput.Search)
                };
            }
            catch (Exception ex)
            {
                return new AdminServiceLayerListOutput<MobileProductOutput>
                {
                    IsExistException = true,
                    ErrorMessages = [ex.Message]
                };
            }
        }




    }
}
