using Moq;
using ProductsCRUD.Application.DTOs.Input;
using ProductsCRUD.Application.S_AuthenticationService;
using ProductsCRUD.Application.S_CipherService;
using ProductsCRUD.Domain._core;
using ProductsCRUD.Domain.AdminUsers;
using Xunit;

namespace ProductsCRUD.Tests.Services.Authentication
{
    public class AuthenticationServiceTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ICipherService> _mockCipherService;
        private readonly AuthenticationService _authenticationService;
        public AuthenticationServiceTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCipherService = new Mock<ICipherService>();
            _authenticationService = new AuthenticationService(_mockUnitOfWork.Object, _mockCipherService.Object);
        }



        [Fact]
        public async Task Login_ShouldReturnSuccess_WhenUserNamePassword_Ok()
        {
            // Arrange
            LoginInput loginInput = new()
            {
                UserName = "admin",
                Password = "admin",
            };

            AdminUser adminUser = new()
            {
                Id = 1,
                UserName = "admin",
                Password = "1N5IJ8IQP4VDV3RiEBAWWQ==",  // encrypted password
                IsActive = true
            };

            string decryptPassword = "admin";

            // Setup the mocks
            _mockUnitOfWork.Setup(uow => uow.AdminUserRepository.GetWithNoTracking(u => u.UserName.ToLower().Equals(loginInput.UserName.ToLower()))).ReturnsAsync(adminUser);
            _mockCipherService.Setup(cs => cs.Decrypt(adminUser.Password)).Returns(decryptPassword);

            // Act
            var result = await _authenticationService.Login(loginInput);


            // Assert
            Assert.True(result.Success);  // Ensure the success flag is true
            Assert.Equal(result.Data.Id, adminUser.Id);


            // Verify interactions
            _mockUnitOfWork.Verify(uow => uow.AdminUserRepository.GetWithNoTracking(u => u.UserName.ToLower().Equals(loginInput.UserName.ToLower())), Times.Once);
            _mockCipherService.Verify(cs => cs.Decrypt(adminUser.Password), Times.Once);
        }


        [Fact]
        public async Task Login_ShouldReturnNotSuccess_WhenUserName_NotExist()
        {
            // Arrange
            LoginInput loginInput = new()
            {
                UserName = "admin",
                Password = "admin",
            };

            AdminUser adminUser = null;

            // Setup the mocks
            _mockUnitOfWork.Setup(uow => uow.AdminUserRepository.GetWithNoTracking(u => u.UserName.ToLower().Equals(loginInput.UserName.ToLower()))).ReturnsAsync(adminUser);


            // Act
            var result = await _authenticationService.Login(loginInput);


            // Assert
            Assert.False(result.Success);


            // Verify interactions
            _mockUnitOfWork.Verify(uow => uow.AdminUserRepository.GetWithNoTracking(u => u.UserName.ToLower().Equals(loginInput.UserName.ToLower())), Times.Once);
        }


        [Fact]
        public async Task Login_ShouldReturnNotSuccess_WhenPassword_Wrong()
        {
            // Arrange
            LoginInput loginInput = new()
            {
                UserName = "admin",
                Password = "admin",
            };

            AdminUser adminUser = new()
            {
                Id = 1,
                UserName = "admin",
                Password = "1N5IJ8IQP4VDV3RiEBAWWQ==",  // encrypted password
                IsActive = true
            };

            string decryptPassword = "test";

            // Setup the mocks
            _mockUnitOfWork.Setup(uow => uow.AdminUserRepository.GetWithNoTracking(u => u.UserName.ToLower().Equals(loginInput.UserName.ToLower()))).ReturnsAsync(adminUser);
            _mockCipherService.Setup(cs => cs.Decrypt(adminUser.Password)).Returns(decryptPassword);


            // Act
            var result = await _authenticationService.Login(loginInput);


            // Assert
            Assert.False(result.Success);
            Assert.NotEqual(decryptPassword, loginInput.Password);


            // Verify interactions
            _mockUnitOfWork.Verify(uow => uow.AdminUserRepository.GetWithNoTracking(u => u.UserName.ToLower().Equals(loginInput.UserName.ToLower())), Times.Once);
            _mockCipherService.Verify(cs => cs.Decrypt(adminUser.Password), Times.Once);
        }


        [Fact]
        public async Task Login_ShouldReturnError_WhenExceptionOccurs()
        {
            // Arrange
            LoginInput loginInput = new()
            {
                UserName = "admin",
                Password = "admin",
            };

            
            // Setup the mocks
            _mockUnitOfWork.Setup(uow => uow.AdminUserRepository.GetWithNoTracking(u => u.UserName.ToLower().Equals(loginInput.UserName.ToLower())))
                .ThrowsAsync(new Exception("Something went wrong"));


            // Act
            var result = await _authenticationService.Login(loginInput);


            // Assert
            Assert.True(result.IsExistException);  // Ensure the exception flag is true
            Assert.Contains("Something went wrong", result.ErrorMessages);  // Ensure error message is returned


            // Verify interactions
            _mockUnitOfWork.Verify(uow => uow.AdminUserRepository.GetWithNoTracking(u => u.UserName.ToLower().Equals(loginInput.UserName.ToLower())), Times.Once);
        }




    }
}
