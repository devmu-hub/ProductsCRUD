using ProductsCRUD.Application.DTOs.Input;
using ProductsCRUD.Application.DTOs.Output;
using ProductsCRUD.Domain._core;
using ProductsCRUD.Application.S_CipherService;
using ProductsCRUD.Domain.AdminUsers;

namespace ProductsCRUD.Application.S_AuthenticationService
{
    public class AuthenticationService(IUnitOfWork unitOfWork,
        ICipherService cipherService) : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICipherService _cipherService = cipherService;



        public async Task<AdminServiceLayerOutput<LoginOutput>> Login(LoginInput loginInput)
        {
            try
            {
                #region Validate Inputs

                if (loginInput == null || string.IsNullOrEmpty(loginInput.UserName) || string.IsNullOrEmpty(loginInput.Password))
                    return new AdminServiceLayerOutput<LoginOutput>
                    {
                        ErrorMessages = ["Invalid input"]
                    };

                #endregion

                AdminUser adminUser = await _unitOfWork.AdminUserRepository.GetWithNoTracking(u => u.UserName.ToLower().Equals(loginInput.UserName.ToLower()));

                if (adminUser is null)
                    return new AdminServiceLayerOutput<LoginOutput>
                    {
                        ErrorMessages = ["User does not exist"]
                    };

                if (!adminUser.IsActive)
                    return new AdminServiceLayerOutput<LoginOutput>
                    {
                        ErrorMessages = ["This user is deactivated"]
                    };

                string decryptPassword = _cipherService.Decrypt(adminUser.Password);

                if (!decryptPassword.Equals(loginInput.Password))
                    return new AdminServiceLayerOutput<LoginOutput>
                    {
                        ErrorMessages = ["Password combination is wrong"]
                    };

                return new AdminServiceLayerOutput<LoginOutput>
                {
                    Success = true,
                    Data = new LoginOutput
                    {
                        Id = adminUser.Id
                    }
                };
            }
            catch (Exception ex)
            {
                return new AdminServiceLayerOutput<LoginOutput>
                {
                    IsExistException = true,
                    ErrorMessages = [ex.Message]
                };
            }
        }



    }
}
