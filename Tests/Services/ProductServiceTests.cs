using Infrastructure.Services;

namespace Tests.Services
{
    public class ProductServiceTests
    {
        private readonly ProductService _productService;
        public ProductServiceTests()
        {
            _productService = new ProductService();
        }
        [Fact]
        public void GetProductName_ValidId_ReturnsProductName()
        {
            // Arrange
            int productId = 1;
            // Act
            var result = _productService.GetProductName(productId);
            // Assert
            Assert.Equal("Product Name for ID 1", result);
        }
        [Fact]
        public void GetProductName_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            int productId = 0; // Invalid ID
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _productService.GetProductName(productId));
            Assert.Equal("Product ID must be greater than zero. (Parameter 'productId')", exception.Message);
        }
    }
}
