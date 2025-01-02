using Microsoft.EntityFrameworkCore;
using ProductsCRUD.Application.DTOs.Input;
using ProductsCRUD.Application.S_AuthenticationService;
using ProductsCRUD.Application.S_CipherService;
using ProductsCRUD.Data.EntityFrameworkCore.Context;
using ProductsCRUD.Data.EntityFrameworkCore.Repositories._core;
using Xunit;

namespace ProductsCRUD.Tests.IntegrationTests
{
    public class AuthenticationServiceIntegrationTests
    {
        private readonly AuthenticationService _authenticationService;
        private readonly ApplicationDbContext _context;
        private readonly ICipherService _cipherService;
        public AuthenticationServiceIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);

            UnitOfWork unitOfWork = new(_context);

            _cipherService = new CipherService();

            _authenticationService = new AuthenticationService(unitOfWork, _cipherService);
        }



        [Fact]
        public async Task Login_ShouldReturnSuccess_WhenUserNamePassword_Ok()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();  // Clear the database before each test
            await InsertSeedAdminUserAsync();


            // Act
            var result = await _authenticationService.Login(new LoginInput
            {
                UserName = "admin",
                Password = "admin"
            });


            // Assert
            Assert.True(result.Success);
            Assert.Equal(1, result.Data.Id);  // return user id equals 1
        }


        [Fact]
        public async Task Login_ShouldReturnNotSuccess_WhenUserName_NotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();  // Clear the database before each test
            await InsertSeedAdminUserAsync();


            // Act
            var result = await _authenticationService.Login(new LoginInput
            {
                UserName = "test", // user name not exist
                Password = "admin"
            });


            // Assert
            Assert.False(result.Success);
            Assert.NotNull(result.ErrorMessages);
            Assert.NotEmpty(result.ErrorMessages);
            Assert.Contains(result.ErrorMessages, error => error.Contains("User does not exist"));
        }


        [Fact]
        public async Task Login_ShouldReturnNotSuccess_WhenPassword_Wrong()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();  // Clear the database before each test
            await InsertSeedAdminUserAsync();


            // Act
            var result = await _authenticationService.Login(new LoginInput
            {
                UserName = "admin",
                Password = "123456" // invalid password
            });


            // Assert
            Assert.False(result.Success);
            Assert.NotNull(result.ErrorMessages);
            Assert.NotEmpty(result.ErrorMessages);
            Assert.Contains(result.ErrorMessages, error => error.Contains("Password combination is wrong"));
        }







        private async Task InsertSeedAdminUserAsync()
        {
            await _context.AdminUsers.AddAsync(new Domain.AdminUsers.AdminUser
            {
                Id = 1,
                UserName = "admin",
                Password = "1N5IJ8IQP4VDV3RiEBAWWQ==",
                IsActive = true
            });

            await _context.SaveChangesAsync();
        }


    }
}
