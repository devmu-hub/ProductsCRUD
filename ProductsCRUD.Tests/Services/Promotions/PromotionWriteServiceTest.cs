using AutoMapper;
using Moq;
using ProductsCRUD.Application.DTOs.Input;
using ProductsCRUD.Application.S_PromotionService.Write;
using ProductsCRUD.Domain._core;
using ProductsCRUD.Domain.Promotions;
using Xunit;

namespace ProductsCRUD.Tests.Services.Promotions
{
    public class PromotionWriteServiceTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly PromotionWriteService _promotionWriteService;
        public PromotionWriteServiceTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _promotionWriteService = new PromotionWriteService(_mockMapper.Object, _mockUnitOfWork.Object);
        }



        [Fact]
        public async Task AddPromotion_ShouldReturnSuccess_WhenPromotionIsAdded()
        {
            // Arrange
            PromotionInput promotionInput = new()
            {
                PromotionTypeId = 1,
                Name = "Promotion 1",
                DiscountPercentage = 10
            };

            Promotion promotion = new()
            {
                PromotionTypeId = 1,
                Name = "Promotion 1",
                DiscountPercentage = 10
            };

            // Setup the mocks
            _mockMapper.Setup(m => m.Map<Promotion>(promotionInput)).Returns(promotion);
            _mockUnitOfWork.Setup(u => u.PromotionRepository.Add(It.IsAny<Promotion>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.Complete()).Returns(Task.CompletedTask);

            // Act
            var result = await _promotionWriteService.Create(promotionInput);

            // Assert
            Assert.True(result.Success);  // Ensure the success flag is true
            _mockUnitOfWork.Verify(u => u.PromotionRepository.Add(It.IsAny<Promotion>()), Times.Once);  // Ensure Add method is called once
            _mockUnitOfWork.Verify(u => u.Complete(), Times.Once);  // Ensure complete is called once
        }


        [Fact]
        public async Task AddPromotion_ShouldReturnError_WhenExceptionOccurs()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.PromotionRepository.Add(It.IsAny<Promotion>())).ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _promotionWriteService.Create(new PromotionInput
            {
                PromotionTypeId = 1,
                Name = "Product 1",
                DiscountPercentage = 10
            });

            // Assert
            Assert.True(result.IsExistException);  // Ensure the exception flag is true
            Assert.Contains("Something went wrong", result.ErrorMessages);  // Ensure error message is returned
        }


        [Fact]
        public async Task AddPromotion_ShouldReturnErrorMessage_WhenPromotionTypeId_NotExists()
        {
            // Arrange
            PromotionInput promotionInput = new()
            {
                PromotionTypeId = 5,  // promotion type id is not exists
                Name = "Promotion 1",
                DiscountPercentage = 10
            };

            _mockUnitOfWork.Setup(uow => uow.PromotionTypeRepository.Any(x => x.Id == promotionInput.PromotionTypeId)).ReturnsAsync(false);

            // Act
            var result = await _promotionWriteService.Create(promotionInput);

            // Assert
            Assert.False(result.Success);
            Assert.Single(result.ErrorMessages);


            // Assert
            _mockUnitOfWork.Verify(uow => uow.PromotionRepository.Add(It.IsAny<Promotion>()), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }


        [Fact]
        public async Task Update_ShouldReturnSuccess_WhenPromotionExists()
        {
            // Arrange
            Promotion promotion = new() { Id = 1, PromotionTypeId = 1, Name = "Updated Name", DiscountPercentage = 15 };

            PromotionInput promotionInput = new() { Id = 1, PromotionTypeId = 1, Name = "Updated Name", DiscountPercentage = 15 };

            _mockUnitOfWork.Setup(uow => uow.PromotionRepository.Get(p => p.Id == promotionInput.Id)).ReturnsAsync(promotion);


            // Act
            var result = await _promotionWriteService.Update(promotionInput);


            // Assert
            Assert.True(result.Success);


            // Verify interactions
            _mockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }


        [Fact]
        public async Task Update_ReturnsNotSuccess_WhenPromotionNotExist()
        {
            // Arrange
            int promotionId = 1;

            Promotion promotion = null;  // Simulate that the promotion was not found

            PromotionInput promotionInput = new() { Id = promotionId, PromotionTypeId = 1, Name = "Updated Name", DiscountPercentage = 35 };

            _mockUnitOfWork.Setup(uow => uow.PromotionRepository.Get(p => p.Id == promotionId)).ReturnsAsync(promotion);


            // Act
            var result = await _promotionWriteService.Update(promotionInput);


            // Assert
            Assert.False(result.Success);


            // Verify interactions
            _mockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }


        [Fact]
        public async Task Delete_ReturnsTrue_WhenPromotionExists()
        {
            // Arrange
            int promotionId = 1;

            Promotion promotion = new() { Id = promotionId, Name = "Promotion 1", DiscountPercentage = 15 };

            _mockUnitOfWork.Setup(uow => uow.PromotionRepository.Get(p => p.Id == promotionId)).ReturnsAsync(promotion);


            // Act
            var result = await _promotionWriteService.Delete(promotionId);


            // Assert
            Assert.True(result.Success);


            // Verify interactions
            _mockUnitOfWork.Verify(uow => uow.PromotionRepository.Get(p => p.Id == promotionId), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }


        [Fact]
        public async Task Delete_ReturnsFalse_WhenPromotionNotExist()
        {
            // Arrange
            int promotionId = 1;

            Promotion promotion = null;

            _mockUnitOfWork.Setup(uow => uow.PromotionRepository.Get(p => p.Id == promotionId)).ReturnsAsync(promotion);


            // Act
            var result = await _promotionWriteService.Delete(promotionId);


            // Assert
            Assert.False(result.Success);


            // Verify interactions
            _mockUnitOfWork.Verify(uow => uow.PromotionRepository.Get(p => p.Id == promotionId), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }





    }
}
