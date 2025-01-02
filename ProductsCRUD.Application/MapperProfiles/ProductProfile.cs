using ProductsCRUD.Application.DTOs.Input;
using ProductsCRUD.Application.DTOs.Output;
using AutoMapper;
using ProductsCRUD.Domain.Products;

namespace ProductsCRUD.Application.MapperProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductInput, Product>();

            CreateMap<Product, ProductOutput>();

            CreateMap<Product, MobileProductOutput>()
                .ForMember(dest =>
                    dest.Promotions,
                    opt => opt.MapFrom(src => src.ProductPromotions.Select(pp => pp.Promotion.Name)));

        }
    }
}
