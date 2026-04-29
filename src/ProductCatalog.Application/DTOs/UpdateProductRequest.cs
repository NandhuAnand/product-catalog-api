namespace ProductCatalog.Application.DTOs;

public record UpdateProductRequest(
    string ProductName,
    string ModifiedBy);