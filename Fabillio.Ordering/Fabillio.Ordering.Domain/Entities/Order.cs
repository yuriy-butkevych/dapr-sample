using System;
using System.Collections.Generic;
using System.Linq;
using Fabillio.Common.Events.Abstractions;
using Fabillio.Ordering.Public.Events;

namespace Fabillio.Ordering.Domain.Entities;

public class Order: IEventsDomainModel
{
    public string Id => GetDocumentId(OrderId);

    public static string GetDocumentId(Guid orderId)
        => "orders/" + orderId;
    
    public Guid OrderId { get; set; }
    
    public Guid CustomerId { get; set; }
    
    public List<Fabillio.Ordering.Domain.EntitiesNodes.OrderItem> Items { get; set; }

    public List<IEvent> Events { get; set; } = new();
    
    public DateTime Created { get; set; }
    
    public DateTime? Updated { get; set; }

    public void Create(Guid customerId, List<Fabillio.Ordering.Domain.EntitiesNodes.OrderItem> items)
    {
        OrderId = Guid.NewGuid();
        Created = DateTime.UtcNow;
        CustomerId = customerId;
        Items = items;

        var eventItems = Items
            .Select(x => new OrderItem
                { ProductId = x.ProductId, Count = x.Count }).ToList();
        
        Events.Add(new OrderPlaced { Items = eventItems, OrderId = OrderId });
    }

    public void Update(List<Fabillio.Ordering.Domain.EntitiesNodes.OrderItem> items)
    {
        Items = items;
        Updated = DateTime.UtcNow;
        
        // TODO: publish event to update inventory
    }
}