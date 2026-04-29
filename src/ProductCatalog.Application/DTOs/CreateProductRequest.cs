namespace ProductCatalog.Application.DTOs;

public record CreateProductRequest(
    string ProductName,
    string CreatedBy,
    int InitialQuantity);