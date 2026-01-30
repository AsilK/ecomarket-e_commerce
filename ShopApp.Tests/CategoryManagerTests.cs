using Moq;
using ShopApp.Business.Concrete;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;

namespace ShopApp.Tests;

public class CategoryManagerTests
{
    private readonly Mock<ICategoryDal> _mockCategoryDal;
    private readonly CategoryManager _categoryManager;

    public CategoryManagerTests()
    {
        _mockCategoryDal = new Mock<ICategoryDal>();
        _categoryManager = new CategoryManager(_mockCategoryDal.Object);
    }

    [Fact]
    public void GetById_WithValidId_ReturnsCategory()
    {
        // Arrange
        var expectedCategory = new Category { Id = 1, Name = "Electronics" };
        _mockCategoryDal.Setup(x => x.GetById(1)).Returns(expectedCategory);

        // Act
        var result = _categoryManager.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Electronics", result.Name);
    }

    [Fact]
    public void GetAll_ReturnsCategoryList()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Electronics" },
            new Category { Id = 2, Name = "Clothing" }
        };
        _mockCategoryDal.Setup(x => x.GetAll(null)).Returns(categories);

        // Act
        var result = _categoryManager.GetAll();

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void Create_CallsRepository()
    {
        // Arrange
        var category = new Category { Name = "New Category" };

        // Act
        _categoryManager.Create(category);

        // Assert
        _mockCategoryDal.Verify(x => x.Create(category), Times.Once);
    }

    [Fact]
    public void Delete_CallsRepository()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "To Delete" };

        // Act
        _categoryManager.Delete(category);

        // Assert
        _mockCategoryDal.Verify(x => x.Delete(category), Times.Once);
    }

    [Fact]
    public void Update_CallsRepository()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Updated" };

        // Act
        _categoryManager.Update(category);

        // Assert
        _mockCategoryDal.Verify(x => x.Update(category), Times.Once);
    }
}
