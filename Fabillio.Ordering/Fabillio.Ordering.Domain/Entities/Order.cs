using System;
using System.Collections.Generic;
using Fabillio.Ordering.Domain.EntitiesNodes;
using Fabillio.Common.Events.Abstractions;

namespace Fabillio.Ordering.Domain.Entities;

public class Order: IEventsDomainModel
{
    public string Id => GetDocumentId(OrderId);

    public static string GetDocumentId(Guid orderId)
        => "orders/" + orderId;
    
    public Guid OrderId { get; set; }
    
    public Guid CustomerId { get; set; }
    
    public List<OrderItem> Items { get; set; }
    
    public List<IEvent> Events { get; set; }
}