using ProductsCRUD.Application.DTOs.Input;
using ProductsCRUD.Application.DTOs.Output;
using AutoMapper;
using ProductsCRUD.Domain.Promotions;

namespace ProductsCRUD.Application.MapperProfiles
{
    public class PromotionProfile : Profile
    {
        public PromotionProfile()
        {
            CreateMap<PromotionInput, Promotion>();

            CreateMap<Promotion, PromotionOutput>()
                .ForMember(dest =>
                    dest.PromotionType,
                    opt => opt.MapFrom(src => src.PromotionType.Name));

            CreateMap<PromotionType, PromotionTypeOutput>();


        }
    }
}
