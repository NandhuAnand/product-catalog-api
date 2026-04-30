using ProductCatalog.Application.DTOs;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application.Mapping;

public static class ProductMapper
{
    public static ProductDto ToDto(Product product)
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