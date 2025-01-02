using AutoMapper;
using Moq;
using ProductsCRUD.Application.DTOs.Input;
using ProductsCRUD.Application.DTOs.Output;
using ProductsCRUD.Application.S_ProductService.Read;
using ProductsCRUD.Domain._core;
using ProductsCRUD.Domain.Products;
using Xunit;

namespace ProductsCRUD.Tests.Services.Products
{
    public class ProductReadServiceTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ProductReadService _productReadService;
        public ProductReadServiceTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _productReadService = new ProductReadService(_mockMapper.Object, _mockUnitOfWork.Object);
        }



        [Fact]
        public async Task GetAll_ShouldReturnSuccess_WhenProductsExist()
        {
            // Arrange
            List<Product> products =
            [
                new() { Id = 1, Name = "Product 1", Description = "Product 1", Price = 100, IsFeatured = false },
                new() { Id = 2, Name = "Product 2", Description = "Product 2", Price = 80, IsFeatured = false },
            ];

            List<ProductOutput> productOutputs =
            [
                new() { Id = 1, Name = "Product 1", Description = "Product 1", Price = 100, IsFeatured = false },
                new() { Id = 2, Name = "Product 2", Description = "Product 2", Price = 80, IsFeatured = false },
            ];

            _mockUnitOfWork.Setup(u => u.ProductRepository.GetAll()).ReturnsAsync(products);

            _mockMapper.Setup(m => m.Map<IEnumerable<ProductOutput>>(products)).Returns(productOutputs);


            // Act
            var result = await _productReadService.GetAll();


            // Assert
            Assert.True(result.Success);  // Check Success is true
            Assert.Equal(2, result.Data.Count());  // Check the count is correct
            Assert.Equal(productOutputs, result.Data);  // Check Data matches the expected output
        }


        [Fact]
        public async Task GetAll_ShouldReturnError_WhenExceptionOccurs()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.ProductRepository.GetAll()).ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _productReadService.GetAll();

            // Assert
            Assert.True(result.IsExistException);  // Check if exception flag is true
            Assert.Contains("Something went wrong", result.ErrorMessages);  // Check if error message is as expected
        }


        [Fact]
        public async Task GetById_ReturnsMappedProduct_WhenProductExists()
        {
            // Arrange
            int productId = 1;

            Product product = new()
            {
                Id = 1,
                Name = "Product 1",
                Description = "Product 1",
                Price = 100,
                IsFeatured = false
            };

            ProductOutput productOutput = new()
            {
                Id = 1,
                Name = "Product 1",
                Description = "Product 1",
                Price = 100,
                IsFeatured = false
            };

            _mockUnitOfWork.Setup(u => u.ProductRepository.GetWithNoTracking(p => p.Id == productId)).ReturnsAsync(product);

            _mockMapper.Setup(m => m.Map<ProductOutput>(product)).Returns(productOutput);


            // Act
            var result = await _productReadService.Get(productId);


            // Assert
            Assert.True(result.Success);
            Assert.Equal("Product 1", result.Data.Name);
            Assert.Equal(productId, result.Data.Id);


            // Verify interactions
            _mockUnitOfWork.Verify(uow => uow.ProductRepository.GetWithNoTracking(p => p.Id == productId), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<ProductOutput>(product), Times.Once);
        }


        [Fact]
        public async Task GetById_ReturnsNull_WhenProductNotFound()
        {
            // Arrange
            int productId = 1;

            Product product = null;  // Simulate that the product was not found

            _mockUnitOfWork.Setup(u => u.ProductRepository.GetWithNoTracking(p => p.Id == productId)).ReturnsAsync(product);


            // Act
            var result = await _productReadService.Get(productId);


            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);


            // Verify interactions
            _mockUnitOfWork.Verify(uow => uow.ProductRepository.GetWithNoTracking(p => p.Id == productId), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<ProductOutput>(product), Times.Never);  // Should not be called if product is null
        }


        [Fact]
        public async Task GetProductsForMobile_ShouldReturnPagedResults_WhenNoException()
        {
            // Arrange
            int pageNumber = 1, pageSize = 2;

            bool? isFeatued = null;

            bool? isNew = null;

            string search = "";

            List<Product> products =
            [
                new() { Id = 1, Name = "Product 1" },
                new() { Id = 2, Name = "Product 2" }
            ];

            List<MobileProductOutput> mobileProductOutputs =
            [
                new() { Id = 1, Name = "Product 1" },
                new() { Id = 2, Name = "Product 2" }
            ];

            _mockUnitOfWork.Setup(u => u.ProductRepository.GetProductsForMobileAsync(isFeatued, isNew, search, (pageNumber - 1) * pageSize, pageSize)).ReturnsAsync(products);
            _mockUnitOfWork.Setup(u => u.ProductRepository.CountProductsForMobileAsync(isFeatued, isNew, search)).ReturnsAsync(5); // Total items
            _mockMapper.Setup(m => m.Map<IEnumerable<MobileProductOutput>>(products)).Returns(mobileProductOutputs);

            // Act
            var result = await _productReadService.GetProductsForMobile(new MobileProductSearchInput
            {
                IsFeatured = isFeatued,
                Search = search,
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            // Assert
            Assert.True(result.Success);
            Assert.False(result.IsExistException);
            Assert.Equal(mobileProductOutputs, result.Data);
            Assert.Equal(5, result.Count); // Total count
        }


        [Fact]
        public async Task GetProductsForMobile_ShouldReturnError_WhenExceptionThrown()
        {
            // Arrange
            int pageNumber = 1, pageSize = 2;

            int skip = (pageNumber - 1) * pageSize;

            bool? isFeatued = null;

            bool? isNew = null;

            string search = "Product";

            string exceptionMessage = "Test exception";

            _mockUnitOfWork.Setup(u => u.ProductRepository.GetProductsForMobileAsync(isFeatued, isNew, search, skip, pageSize))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _productReadService.GetProductsForMobile(new MobileProductSearchInput
            {
                IsFeatured = isFeatued,
                Search = search,
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            // Assert
            Assert.False(result.Success);
            Assert.True(result.IsExistException);
            Assert.Contains(exceptionMessage, result.ErrorMessages);
        }




    }
}
