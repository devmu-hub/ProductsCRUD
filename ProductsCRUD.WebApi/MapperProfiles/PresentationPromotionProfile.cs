using AutoMapper;
using ProductsCRUD.Application.DTOs.Input;
using ProductsCRUD.Application.DTOs.Output;
using ProductsCRUD.WebApi.HTTPModels.Requests;
using ProductsCRUD.WebApi.HTTPModels.Responses;

namespace ProductsCRUD.WebApi.MapperProfiles
{
    public class PresentationPromotionProfile : Profile
    {
        public PresentationPromotionProfile()
        {
            CreateMap<PromotionRequest, PromotionInput>();

            CreateMap<PromotionOutput, PromotionResponse>();

            CreateMap<PromotionTypeOutput, PromotionTypeResponse>();


        }
    }
}
