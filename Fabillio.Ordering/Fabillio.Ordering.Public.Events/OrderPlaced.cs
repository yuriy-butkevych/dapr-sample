using System;
using System.Collections.Generic;
using Fabillio.Common.Events.Abstractions;

namespace Fabillio.Ordering.Public.Events;

public class OrderPlaced: IEvent
{
    public DateTime When { get; } = DateTime.UtcNow;
    
    public Guid OrderId { get; set; }

    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
}

public class OrderItem
{
    public Guid ProductId { get; set; }
    
    public int Count { get; set; }
}