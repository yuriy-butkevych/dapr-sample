namespace Fabillio.Inventory.Public.Contracts.Requests;

public class CreateNewProductRequest
{
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public decimal Price { get; set; }

    public int RemainingCount { get; set; }
}