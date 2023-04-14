using System.Collections.Generic;
using System.Threading.Tasks;
using Fabillio.Inventory.API.Constants;
using Fabillio.Inventory.Cqrs.Commands;
using Fabillio.Inventory.Cqrs.Queries;
using Fabillio.Inventory.Public.Contracts.Requests;
using Fabillio.Inventory.Public.Contracts.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fabillio.Inventory.API.Controllers;

/// <summary>
/// Products inventory
/// </summary>
[ApiExplorerSettings(GroupName = RoutingConstants.Documentation._clientInterface)]
[Route(RoutingConstants.Products._base)]
public class ProductsController: BaseController
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductResponse>))]
    public async Task<IActionResult> GetAllProducts()
    {
        var query = new GetAllProductsQuery();

        var result = await Mediator.Send(query);

        return Ok(result);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductResponse))]
    public async Task<IActionResult> CreateProduct([FromBody]CreateNewProductRequest request)
    {
        var command = new CreateProductCommand(request.Title, request.Description, request.Price, request.RemainingCount);

        var result = await Mediator.Send(command);

        return Ok(result);
    }
}