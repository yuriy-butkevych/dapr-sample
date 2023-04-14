using System.Threading;
using System.Threading.Tasks;
using Fabillio.Common.Events.Abstractions;
using Fabillio.Ordering.Public.Events;

namespace Fabillio.Inventory.Cqrs.EventHandlers;

public class UpdateProductsRemainingCountV2: IHandle<OrderPlaced>
{
    public Task Handle(OrderPlaced @event, CancellationToken cancellationToken)
    {
        return Task.FromResult(0);
    }
}