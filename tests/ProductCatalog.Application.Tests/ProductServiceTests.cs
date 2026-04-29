using Moq;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Application.Services;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Application.Exceptions;

namespace ProductCatalog.Application.Tests;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _service = new ProductService(
            _repositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task CreateAsync_Should_Create_Product()
    {
        var request = new CreateProductRequest("Test Product", "admin", 5);

        var result = await _service.CreateAsync(request, CancellationToken.None);

        Assert.Equal("Test Product", result.ProductName);

        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Product()
    {
        var product = new Product("Test", "admin");

        typeof(Product).GetProperty("Id")!.SetValue(product, 1);

        _repositoryMock
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var result = await _service.GetByIdAsync(1, CancellationToken.None);

        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Throw_NotFound()
    {
        _repositoryMock
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.GetByIdAsync(1, CancellationToken.None));
    }


}