using System;
using System.Threading.Tasks;
using Dapr.Actors;
using Dapr.Actors.Runtime;
using Fabillio.Inventory.Domain.Entities;
using Raven.Client.Documents;

namespace Fabillio.Inventory.Cqrs.Actors;

public interface IProductActor : IActor
{
    public Task BookProducts(int count);
}

public class ProductActor : Actor, IProductActor
{
    private readonly IDocumentStore _documentStore;
    
    public ProductActor(ActorHost host, IDocumentStore documentStore) : base(host)
    {
        _documentStore = documentStore;
    }

    public async Task BookProducts(int count)
    {
        var productId = Guid.Parse(Id.GetId());

        using var session = _documentStore.OpenAsyncSession();
        var product = await session.LoadAsync<Product>(Product.GetDocumentId(productId));
        
        if (product == null)
        {
            return;
        }
  
        product.UpdateRemainingAmount(product.RemainingCount - count);

        await session.SaveChangesAsync();
    }
}