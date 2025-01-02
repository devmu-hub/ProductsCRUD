using AutoMapper;
using ProductsCRUD.Application.DTOs.Input;
using ProductsCRUD.Application.DTOs.Output;
using ProductsCRUD.WebApi.HTTPModels.Requests;
using ProductsCRUD.WebApi.HTTPModels.Responses;

namespace ProductsCRUD.WebApi.MapperProfiles
{
    public class PresentationProductProfile : Profile
    {
        public PresentationProductProfile()
        {
            CreateMap<ProductRequest, ProductInput>();

            CreateMap<AssignPromotionRequest, AssignPromotionInput>();

            CreateMap<ProductOutput, ProductResponse>();

            CreateMap<MobileProductSearchRequest, MobileProductSearchInput>();

            CreateMap<MobileProductOutput, MobileProductResponse>();

        }
    }
}
