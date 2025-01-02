using ProductsCRUD.Application.DTOs.Input;
using ProductsCRUD.Application.DTOs.Output;

namespace ProductsCRUD.Application.S_AuthenticationService
{
    public interface IAuthenticationService
    {
        Task<AdminServiceLayerOutput<LoginOutput>> Login(LoginInput loginInput);
    }
}
