using System.Threading.Tasks;
using Fabillio.Ordering.API.Constants;
using Fabillio.Ordering.Cqrs.Commands;
using Fabillio.Ordering.Public.Contracts.Requests;
using Fabillio.Ordering.Public.Contracts.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fabillio.Ordering.API.Controllers;

/// <summary>
/// Ordering
/// </summary>
[ApiExplorerSettings(GroupName = RoutingConstants.Documentation._clientInterface)]
[Route(RoutingConstants.Orders._base)]
public class OrdersController: BaseController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderResponse))]
    public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequest request)
    {
        var command = new PlaceOrderCommand(request.CustomerId, request.Items);

        var result = await Mediator.Send(command);

        return Ok(result);
    }
}