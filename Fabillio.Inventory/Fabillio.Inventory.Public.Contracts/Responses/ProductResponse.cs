using System;

namespace Fabillio.Inventory.Public.Contracts.Responses;

public class ProductResponse
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }

    public string Description { get; set; }
    
    public int RemainingCount { get; set; }
}