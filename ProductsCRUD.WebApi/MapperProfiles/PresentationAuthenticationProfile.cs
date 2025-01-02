using AutoMapper;
using ProductsCRUD.Application.DTOs.Input;
using ProductsCRUD.WebApi.HTTPModels.Requests;

namespace ProductsCRUD.WebApi.MapperProfiles
{
    public class PresentationAuthenticationProfile : Profile
    {
        public PresentationAuthenticationProfile()
        {
            CreateMap<LoginRequest, LoginInput>();
        }
    }
}
