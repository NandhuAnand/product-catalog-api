namespace ProductCatalog.Application.DTOs;

public record ProductDto(
    int Id,
    string ProductName,
    string CreatedBy,
    DateTime CreatedOn,
    string? ModifiedBy,
    DateTime? ModifiedOn,
    int TotalQuantity);