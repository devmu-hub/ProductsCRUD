using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductsCRUD.Application.DTOs.Input;
using ProductsCRUD.Application.DTOs.Output;
using ProductsCRUD.Application.MapperProfiles;
using ProductsCRUD.Application.S_ProductService.Read;
using ProductsCRUD.Application.S_ProductService.Write;
using ProductsCRUD.Data.EntityFrameworkCore.Context;
using ProductsCRUD.Data.EntityFrameworkCore.Repositories._core;
using ProductsCRUD.Domain.Products;
using Xunit;

namespace ProductsCRUD.Tests.IntegrationTests
{
    public class ProductServiceIntegrationTests
    {
        private readonly ProductWriteService _productWriteService;
        private readonly ProductReadService _productReadService;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ProductServiceIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);

            UnitOfWork unitOfWork = new(_context);

            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<ProductProfile>());
            _mapper = mapperConfig.CreateMapper();

            _productWriteService = new ProductWriteService(_mapper, unitOfWork);
            _productReadService = new ProductReadService(_mapper, unitOfWork);
        }


        [Fact]
        public async Task Create_Adds100Products_Successfully()
        {
            // Arrange
            List<ProductInput> productInputs = SeedProducts();

            var results = new List<AdminServiceLayerOutput<bool>>();


            // Act
            foreach (ProductInput productInput in productInputs)
            {
                var result = await _productWriteService.Create(productInput);

                results.Add(result);
            }


            // Assert
            var savedProducts = await _context.Products.ToListAsync();

            Assert.All(results, r => Assert.True(r.Success, "All products should be successfully added."));
            Assert.Equal(100, savedProducts.Count);
            Assert.Contains(savedProducts, p => p.Name == "Product 1");
            Assert.Contains(savedProducts, p => p.Name == "Product 100");
        }


        [Fact]
        public async Task Create_InvalidProductInput_ReturnsErrorMessage()
        {
            // Arrange
            ProductInput invalidProductInput = new()
            {
                Name = "", // Invalid: Name cannot be empty
                Description = "This is an invalid product",
                Price = -100, // Invalid: Price cannot be negative
            };


            // Act
            var result = await _productWriteService.Create(invalidProductInput);


            // Assert
            Assert.False(result.Success);
            Assert.NotNull(result.ErrorMessages);
            Assert.NotEmpty(result.ErrorMessages);
            Assert.Contains(result.ErrorMessages, error => error.Contains("Name cannot be empty"));
            Assert.Contains(result.ErrorMessages, error => error.Contains("Price cannot be negative"));
        }


        [Fact]
        public async Task Update_ValidProductId_UpdatesProductSuccessfully()
        {
            // Arrange
            Product product = new()
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 100,
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            // Prepare updated data
            ProductInput productInput = new()
            {
                Id = product.Id,
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 150,
            };

            // Act
            var result = await _productWriteService.Update(productInput);

            // Assert
            Assert.True(result.Success);

            var updatedResult = await _productReadService.Get(product.Id);

            Assert.NotNull(updatedResult); // Ensure the product still exists
            Assert.Equal("Updated Product", updatedResult.Data.Name); // Verify the updated name
            Assert.Equal("Updated Description", updatedResult.Data.Description); // Verify the updated description
            Assert.Equal(150, updatedResult.Data.Price); // Verify the updated price
        }


        [Fact]
        public async Task Delete_ValidProductId_RemovesProductSuccessfully()
        {
            // Arrange
            Product product = new()
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 100,
            };

            await _context.Database.EnsureDeletedAsync();  // Clear the database before each test

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            int productId = product.Id; // Retrieve the ID of the added product

            // Act
            var result = await _productWriteService.Delete(productId);

            // Assert
            Assert.True(result.Success);
            Product deletedProduct = await _context.Products.FindAsync(productId);
            Assert.Null(deletedProduct); // Ensure the product was removed
        }


        [Fact]
        public async Task Delete_InvalidProductId_ReturnsErrorMessage()
        {
            // Arrange
            int invalidProductId = 999; // ID not present in the database

            // Act
            var result = await _productWriteService.Delete(invalidProductId);

            // Assert
            Assert.False(result.Success);
            Assert.NotNull(result.ErrorMessages);
            Assert.Contains(result.ErrorMessages, error => error == "Product id is not exist");
        }








        private static List<ProductInput> SeedProducts()
        {
            List<ProductInput> productInputs = [];

            for (int i = 1; i <= 100; i++)
            {
                productInputs.Add(new ProductInput
                {
                    Name = $"Product {i}",
                    Description = $"Description for Product {i}",
                    Price = i * 10m,
                });
            }

            return productInputs;
        }

    }

}
