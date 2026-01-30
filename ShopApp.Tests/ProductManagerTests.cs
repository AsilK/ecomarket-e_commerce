using Moq;
using ShopApp.Business.Concrete;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;

namespace ShopApp.Tests;

public class ProductManagerTests
{
    private readonly Mock<IProductDal> _mockProductDal;
    private readonly ProductManager _productManager;

    public ProductManagerTests()
    {
        _mockProductDal = new Mock<IProductDal>();
        _productManager = new ProductManager(_mockProductDal.Object);
    }

    [Fact]
    public void GetById_WithValidId_ReturnsProduct()
    {
        // Arrange
        var expectedProduct = new Product { Id = 1, Name = "Test Product" };
        _mockProductDal.Setup(x => x.GetById(1)).Returns(expectedProduct);

        // Act
        var result = _productManager.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedProduct.Id, result.Id);
        Assert.Equal(expectedProduct.Name, result.Name);
    }

    [Fact]
    public void GetById_WithInvalidId_ReturnsNull()
    {
        // Arrange
        _mockProductDal.Setup(x => x.GetById(999)).Returns((Product?)null);

        // Act
        var result = _productManager.GetById(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetAll_ReturnsProductList()
    {
        // Arrange
        var expectedProducts = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1" },
            new Product { Id = 2, Name = "Product 2" }
        };
        _mockProductDal.Setup(x => x.GetAll(null)).Returns(expectedProducts);

        // Act
        var result = _productManager.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Create_WithValidProduct_ReturnsTrue()
    {
        // Arrange
        var product = new Product
        {
            Name = "Valid Product",
            ImageUrl = "http://example.com/image.jpg",
            Description = "Valid description"
        };

        // Act
        var result = _productManager.Create(product);

        // Assert
        Assert.True(result);
        _mockProductDal.Verify(x => x.Create(product), Times.Once);
    }

    [Fact]
    public void Create_WithEmptyName_ReturnsFalse()
    {
        // Arrange
        var product = new Product
        {
            Name = "",
            ImageUrl = "http://example.com/image.jpg",
            Description = "Valid description"
        };

        // Act
        var result = _productManager.Create(product);

        // Assert
        Assert.False(result);
        _mockProductDal.Verify(x => x.Create(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public void Create_WithEmptyImageUrl_ReturnsFalse()
    {
        // Arrange
        var product = new Product
        {
            Name = "Valid Product",
            ImageUrl = "",
            Description = "Valid description"
        };

        // Act
        var result = _productManager.Create(product);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Create_WithEmptyDescription_ReturnsFalse()
    {
        // Arrange
        var product = new Product
        {
            Name = "Valid Product",
            ImageUrl = "http://example.com/image.jpg",
            Description = ""
        };

        // Act
        var result = _productManager.Create(product);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Delete_CallsRepository()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Test" };

        // Act
        _productManager.Delete(product);

        // Assert
        _mockProductDal.Verify(x => x.Delete(product), Times.Once);
    }

    [Fact]
    public void Update_CallsRepository()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Updated Product" };

        // Act
        _productManager.Update(product);

        // Assert
        _mockProductDal.Verify(x => x.Update(product), Times.Once);
    }

    [Fact]
    public void GetProductsByCategory_ReturnsFilteredProducts()
    {
        // Arrange
        var categoryProducts = new List<Product>
        {
            new Product { Id = 1, Name = "Category Product" }
        };
        _mockProductDal.Setup(x => x.GetProductsByCategory("electronics", 1, 10))
            .Returns(categoryProducts);

        // Act
        var result = _productManager.GetProductsByCategory("electronics", 1, 10);

        // Assert
        Assert.Single(result);
        Assert.Equal("Category Product", result[0].Name);
    }
}
