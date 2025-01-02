using AutoMapper;
using Moq;
using ProductsCRUD.Application.DTOs.Input;
using ProductsCRUD.Application.S_ProductService.Write;
using ProductsCRUD.Domain._core;
using ProductsCRUD.Domain.Products;
using Xunit;

namespace ProductsCRUD.Tests.Services.Products
{
    public class ProductWriteServiceTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ProductWriteService _productWriteService;
        public ProductWriteServiceTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _productWriteService = new ProductWriteService(_mockMapper.Object, _mockUnitOfWork.Object);
        }



        [Fact]
        public async Task AddProduct_ShouldReturnSuccess_WhenProductIsAdded()
        {
            // Arrange
            ProductInput productInput = new()
            {
                Name = "Product 1",
                Description = "Product 1",
                Price = 50,
                IsFeatured = false,
            };

            Product product = new()
            {
                Name = "Product 1",
                Description = "Product 1",
                Price = 50,
                IsFeatured = false,
            };

            // Setup the mocks
            _mockMapper.Setup(m => m.Map<Product>(productInput)).Returns(product);
            _mockUnitOfWork.Setup(u => u.ProductRepository.Add(It.IsAny<Product>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.Complete()).Returns(Task.CompletedTask);

            // Act
            var result = await _productWriteService.Create(productInput);

            // Assert
            Assert.True(result.Success);  // Ensure the success flag is true
            _mockUnitOfWork.Verify(u => u.ProductRepository.Add(It.IsAny<Product>()), Times.Once);  // Ensure Add method is called once
            _mockUnitOfWork.Verify(u => u.Complete(), Times.Once);  // Ensure complete is called once
        }


        [Fact]
        public async Task AddProduct_ShouldReturnError_WhenExceptionOccurs()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.ProductRepository.Add(It.IsAny<Product>())).ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _productWriteService.Create(new ProductInput
            {
                Name = "Product 1",
                Description = "Product 1",
                Price = 50,
                IsFeatured = false
            });

            // Assert
            Assert.True(result.IsExistException);  // Ensure the exception flag is true
            Assert.Contains("Something went wrong", result.ErrorMessages);  // Ensure error message is returned
        }


        [Fact]
        public async Task Update_ShouldReturnSuccess_WhenProductExists()
        {
            // Arrange
            Product product = new() { Id = 1, Name = "Old name", Description = "Old description", Price = 15 };

            ProductInput productInput = new() { Id = 1, Name = "Old name", Description = "Old description", Price = 15 };

            _mockUnitOfWork.Setup(uow => uow.ProductRepository.Get(p => p.Id == productInput.Id)).ReturnsAsync(product);


            // Act
            var result = await _productWriteService.Update(productInput);


            // Assert
            Assert.True(result.Success);


            // Verify interactions
            _mockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }


        [Fact]
        public async Task Update_ReturnsNotSuccess_WhenProductNotExist()
        {
            // Arrange
            int productId = 1;

            Product product = null;  // Simulate that the promotion was not found

            ProductInput productInput = new() { Id = productId, Name = "Updated name", Description = "Updated description", Price = 25 };

            _mockUnitOfWork.Setup(uow => uow.ProductRepository.Get(p => p.Id == productId)).ReturnsAsync(product);


            // Act
            var result = await _productWriteService.Update(productInput);


            // Assert
            Assert.False(result.Success);


            // Verify interactions
            _mockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }


        [Fact]
        public async Task Delete_ReturnsTrue_WhenProductExists()
        {
            // Arrange
            int productId = 1;

            Product product = new() { Id = productId, Name = "Product 1", Description = "Product 1", Price = 10 };

            _mockUnitOfWork.Setup(uow => uow.ProductRepository.Get(p => p.Id == productId)).ReturnsAsync(product);


            // Act
            var result = await _productWriteService.Delete(productId);


            // Assert
            Assert.True(result.Success);


            // Verify interactions
            _mockUnitOfWork.Verify(uow => uow.ProductRepository.Get(p => p.Id == productId), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }


        [Fact]
        public async Task Delete_ReturnsFalse_WhenProductNotExist()
        {
            // Arrange
            int productId = 1;

            Product product = null;

            _mockUnitOfWork.Setup(uow => uow.ProductRepository.Get(p => p.Id == productId)).ReturnsAsync(product);


            // Act
            var result = await _productWriteService.Delete(productId);


            // Assert
            Assert.False(result.Success);


            // Verify interactions
            _mockUnitOfWork.Verify(uow => uow.ProductRepository.Get(p => p.Id == productId), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }



    }
}
