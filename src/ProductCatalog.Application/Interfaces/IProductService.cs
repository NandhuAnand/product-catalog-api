using ProductCatalog.Application.DTOs;

namespace ProductCatalog.Application.Interfaces;

public interface IProductService
{
    Task<ProductDto> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken);
    Task<ProductDto> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<PagedResponse<ProductDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<ProductDto> UpdateAsync(int id, UpdateProductRequest request, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}