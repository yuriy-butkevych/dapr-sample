using Fabillio.Ordering.API.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Fabillio.Ordering.API.Controllers;

/// <summary>
/// Ordering
/// </summary>
[ApiExplorerSettings(GroupName = RoutingConstants.Documentation._clientInterface)]
[Route(RoutingConstants.Orders._base)]
public class OrdersController: BaseController
{
    
}