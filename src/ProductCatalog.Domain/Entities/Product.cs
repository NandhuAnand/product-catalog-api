using ProductCatalog.Domain.Exceptions;

namespace ProductCatalog.Domain.Entities;

public class Product
{
    private readonly List<Item> _items = new();

    private Product()
    {
    }

    public Product(string productName, string createdBy)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new DomainException("Product name is required.");

        if (string.IsNullOrWhiteSpace(createdBy))
            throw new DomainException("Created by is required.");

        ProductName = productName;
        CreatedBy = createdBy;
        CreatedOn = DateTime.UtcNow;
    }

    public int Id { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public string CreatedBy { get; private set; } = string.Empty;
    public DateTime CreatedOn { get; private set; }
    public string? ModifiedBy { get; private set; }
    public DateTime? ModifiedOn { get; private set; }

    public IReadOnlyCollection<Item> Items => _items.AsReadOnly();

    public void Update(string productName, string modifiedBy)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new DomainException("Product name is required.");

        if (string.IsNullOrWhiteSpace(modifiedBy))
            throw new DomainException("Modified by is required.");

        ProductName = productName;
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;
    }

    public void AddItem(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");

        _items.Add(new Item(quantity));
    }
}