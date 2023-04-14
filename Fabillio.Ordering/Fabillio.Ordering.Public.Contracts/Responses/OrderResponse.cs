using System;
using System.Collections.Generic;
using Fabillio.Ordering.Domain.EntitiesNodes;

namespace Fabillio.Ordering.Public.Contracts.Responses;

public class OrderResponse
{
    public Guid OrderId { get; set; }
    
    public Guid CustomerId { get; set; }
    
    public List<OrderItem> OrderItems { get; set; }
}