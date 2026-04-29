using ProductCatalog.Domain.Exceptions;

namespace ProductCatalog.Domain.Entities;

public class Item
{
    private Item()
    {
    }

    public Item(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");

        Quantity = quantity;
    }

    public int Id { get; private set; }
    public int ProductId { get; private set; }
    public int Quantity { get; private set; }

    public Product Product { get; private set; } = null!;
}