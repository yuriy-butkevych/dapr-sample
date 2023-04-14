using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapr.Actors;
using Dapr.Actors.Client;
using Fabillio.Common.Events.Abstractions;
using Fabillio.Inventory.Cqrs.Actors;
using Fabillio.Inventory.Domain.Entities;
using Fabillio.Ordering.Public.Events;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace Fabillio.Inventory.Cqrs.EventHandlers;

public class UpdateProductsRemainingCount: IHandle<OrderPlaced>
{
    private readonly IAsyncDocumentSession _documentSession;

    public UpdateProductsRemainingCount(IAsyncDocumentSession documentSession)
    {
        _documentSession = documentSession;
    }
    
    public async Task Handle(OrderPlaced @event, CancellationToken cancellationToken)
    {
        var productIds = @event.Items.Select(x => x.ProductId).ToList();
        var products = await _documentSession.Query<Product>()
            .Where(x => x.ProductId.In(productIds))
            .ToListAsync(cancellationToken);

        foreach (var product in products)
        {
            var placedCount = @event.Items.Where(x => x.ProductId == product.ProductId).Sum(x => x.Count);
            product.UpdateRemainingAmount(product.RemainingCount - placedCount);
        }

        await _documentSession.SaveChangesAsync(cancellationToken);
    }
}

// public class UpdateProductsRemainingCount: IHandle<OrderPlaced>
// {
//     private readonly IAsyncDocumentSession _documentSession;
//     private readonly IActorProxyFactory _actorProxyFactory;
//
//     public UpdateProductsRemainingCount(IAsyncDocumentSession documentSession, IActorProxyFactory actorProxyFactory)
//     {
//         _documentSession = documentSession;
//         _actorProxyFactory = actorProxyFactory;
//     }
//     
//     public async Task Handle(OrderPlaced @event, CancellationToken cancellationToken)
//     {
//         var productIds = @event.Items.Select(x => x.ProductId).ToList();
//         var products = await _documentSession.Query<Product>()
//             .Where(x => x.ProductId.In(productIds))
//             .ToListAsync(cancellationToken);
//
//         foreach (var product in products)
//         {
//             var placedCount = @event.Items
//                 .Where(x => x.ProductId == product.ProductId)
//                 .Sum(x => x.Count);
//             
//             var actorId = new ActorId(product.ProductId.ToString());
//             var actorProxy = _actorProxyFactory.CreateActorProxy<IProductActor>(actorId, nameof(ProductActor));
//             
//             await actorProxy.BookProducts(placedCount);
//         }
//     }
// }