using AutoMapper;
using Moq;
using ProductsCRUD.Application.DTOs.Output;
using ProductsCRUD.Application.S_PromotionService.Read;
using ProductsCRUD.Domain._core;
using ProductsCRUD.Domain.Promotions;
using Xunit;

namespace ProductsCRUD.Tests.Services.Promotions
{
    public class PromotionReadServiceTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly PromotionReadService _promotionReadService;
        public PromotionReadServiceTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _promotionReadService = new PromotionReadService(_mockUnitOfWork.Object, _mockMapper.Object);
        }



        [Fact]
        public async Task GetAll_ShouldReturnSuccess_WhenExist()
        {
            // Arrange
            List<Promotion> promotions =
            [
                new() { Id = 1, PromotionTypeId = 1, Name = "Promotion 1", DiscountPercentage = 15 },
                new() { Id = 2, PromotionTypeId = 1, Name = "Promotion 2", DiscountPercentage = 35 },
            ];

            List<PromotionOutput> promotionOutputs =
            [
                new() { Id = 1, PromotionTypeId = 1, Name = "Promotion 1", DiscountPercentage = 15 },
                new() { Id = 2, PromotionTypeId = 1, Name = "Promotion 2", DiscountPercentage = 35 },
            ];

            _mockUnitOfWork.Setup(u => u.PromotionRepository.GetPromotionsAsync()).ReturnsAsync(promotions);

            _mockMapper.Setup(m => m.Map<IEnumerable<PromotionOutput>>(promotions)).Returns(promotionOutputs);


            // Act
            var result = await _promotionReadService.GetAll();


            // Assert
            Assert.True(result.Success);  // Check Success is true
            Assert.Equal(2, result.Data.Count());  // Check the count is correct
            Assert.Equal(promotionOutputs, result.Data);  // Check Data matches the expected output
        }


        [Fact]
        public async Task GetAll_ShouldReturnError_WhenExceptionOccurs()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.PromotionRepository.GetPromotionsAsync()).ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _promotionReadService.GetAll();

            // Assert
            Assert.True(result.IsExistException);  // Check if exception flag is true
            Assert.Contains("Something went wrong", result.ErrorMessages);  // Check if error message is as expected
        }


        [Fact]
        public async Task GetById_ReturnsMappedProduct_WhenExists()
        {
            // Arrange
            int promotionId = 1;

            Promotion promotion = new()
            {
                Id = 1,
                PromotionTypeId = 1,
                Name = "Promotion 1",
                DiscountPercentage = 15
            };

            PromotionOutput promotionOutput = new()
            {
                Id = 1,
                PromotionTypeId = 1,
                Name = "Promotion 1",
                DiscountPercentage = 15
            };

            _mockUnitOfWork.Setup(u => u.PromotionRepository.GetPromotionByIdAsync(promotionId)).ReturnsAsync(promotion);

            _mockMapper.Setup(m => m.Map<PromotionOutput>(promotion)).Returns(promotionOutput);


            // Act
            var result = await _promotionReadService.Get(promotionId);


            // Assert
            Assert.True(result.Success);
            Assert.Equal("Promotion 1", result.Data.Name);
            Assert.Equal(promotionId, result.Data.Id);


            // Verify interactions
            _mockUnitOfWork.Verify(uow => uow.PromotionRepository.GetPromotionByIdAsync(promotionId), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<PromotionOutput>(promotion), Times.Once);
        }


        [Fact]
        public async Task GetById_ReturnsNull_WhenNotFound()
        {
            // Arrange
            int promotionId = 1;

            Promotion promotion = null;  // Simulate that the promotion was not found

            _mockUnitOfWork.Setup(u => u.PromotionRepository.GetPromotionByIdAsync(promotionId)).ReturnsAsync(promotion);


            // Act
            var result = await _promotionReadService.Get(promotionId);


            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);


            // Verify interactions
            _mockUnitOfWork.Verify(uow => uow.PromotionRepository.GetPromotionByIdAsync(promotionId), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<PromotionOutput>(promotion), Times.Never);  // Should not be called if promotion is null
        }




    }
}
