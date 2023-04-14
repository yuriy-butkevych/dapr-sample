using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Fabillio.Inventory.API.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    private IMediator _mediator;

    protected IMediator Mediator
        => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
}