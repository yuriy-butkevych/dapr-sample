using System;
using System.Collections.Generic;
using Fabillio.Ordering.Domain.EntitiesNodes;

namespace Fabillio.Ordering.Public.Contracts.Requests;

public class PlaceOrderRequest
{
    public Guid CustomerId { get; set; }
    
    public List<OrderItem> Items { get; set; }
}