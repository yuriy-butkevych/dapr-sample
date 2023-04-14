using Dapr.Actors.Runtime;
using Fabillio.Common.Configurations.Extensions;
using Fabillio.Common.Events.Abstractions;
using Raven.Client.Documents;

namespace Fabillio.Ordering.Infrastructure;

public class OrderingOutboxEventsCronActor : OutboxEventsCronActor
{
    public OrderingOutboxEventsCronActor(ActorHost host, IDocumentStore documentStore, IEventPublisher eventPublisher) 
        : base(host, documentStore, eventPublisher)
    {
    }

    protected override TimeSpan ExecutionInterval => TimeSpan.FromMinutes(10);
}