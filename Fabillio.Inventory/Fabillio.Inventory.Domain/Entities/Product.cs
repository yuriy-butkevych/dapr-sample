using System;

namespace Fabillio.Inventory.Domain.Entities;

public class Product
{
    public string Id => GetDocumentId(ProductId);

    public static string GetDocumentId(Guid productId)
        => "products/" + productId;
    
    public Guid ProductId { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public int RemainingCount { get; set; }
    
    public decimal Price { get; set; }
    
    public DateTime Created { get; protected set; }
    
    public DateTime? Updated { get; protected set; }

    public void Create(string title, string description, decimal price, int remainingCount)
    {
        ProductId = Guid.NewGuid();
        Title = title;
        Description = description;
        Price = price;
        RemainingCount = remainingCount;
        Created = DateTime.UtcNow;
    }
    
    public void UpdateRemainingAmount(int remainingCount)
    {
        RemainingCount = remainingCount;
        Updated = DateTime.UtcNow;
    }
}