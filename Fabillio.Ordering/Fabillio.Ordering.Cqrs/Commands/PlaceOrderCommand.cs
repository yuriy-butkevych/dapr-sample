using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fabillio.Ordering.Domain.Entities;
using Fabillio.Ordering.Domain.EntitiesNodes;
using Fabillio.Ordering.Public.Contracts.Responses;
using MediatR;
using Raven.Client.Documents.Session;

namespace Fabillio.Ordering.Cqrs.Commands;

public record PlaceOrderCommand(Guid CustomerId, List<OrderItem> Items): IRequest<OrderResponse>;

internal class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, OrderResponse>
{
    private readonly IAsyncDocumentSession _documentSession;

    public PlaceOrderCommandHandler(IAsyncDocumentSession documentSession)
    {
        _documentSession = documentSession;
    }
    
    public async Task<OrderResponse> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order();
        order.Create(request.CustomerId, request.Items);

        await _documentSession.StoreAsync(order, cancellationToken);
        await _documentSession.SaveChangesAsync(cancellationToken);

        return new OrderResponse
        {
            CustomerId = request.CustomerId,
            OrderId = order.OrderId,
            OrderItems = request.Items
        };
    }
}