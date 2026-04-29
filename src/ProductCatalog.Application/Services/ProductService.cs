using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Exceptions;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductDto> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var product = new Product(request.ProductName, request.CreatedBy);

        if (request.InitialQuantity > 0)
        {
            product.AddItem(request.InitialQuantity);
        }

        await _productRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(product);
    }

    public async Task<ProductDto> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken);

        if (product is null)
            throw new NotFoundException($"Product with id {id} was not found.");

        return MapToDto(product);
    }

    public async Task<PagedResponse<ProductDto>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        var products = await _productRepository.GetPagedAsync(pageNumber, pageSize, cancellationToken);
        var totalCount = await _productRepository.CountAsync(cancellationToken);

        return new PagedResponse<ProductDto>(
            products.Select(MapToDto).ToList(),
            pageNumber,
            pageSize,
            totalCount);
    }

    public async Task<ProductDto> UpdateAsync(int id, UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdForUpdateAsync(id, cancellationToken);

        if (product is null)
            throw new NotFoundException($"Product with id {id} was not found.");

        product.Update(request.ProductName, request.ModifiedBy);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(product);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdForUpdateAsync(id, cancellationToken);

        if (product is null)
            throw new NotFoundException($"Product with id {id} was not found.");

        _productRepository.Delete(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static ProductDto MapToDto(Product product)
    {
        return new ProductDto(
            product.Id,
            product.ProductName,
            product.CreatedBy,
            product.CreatedOn,
            product.ModifiedBy,
            product.ModifiedOn,
            product.Items.Sum(x => x.Quantity));
    }
}