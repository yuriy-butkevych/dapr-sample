using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fabillio.Inventory.Domain.Entities;
using Fabillio.Inventory.Public.Contracts.Responses;
using MediatR;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Fabillio.Inventory.Cqrs.Queries;

public record GetAllProductsQuery:IRequest<List<ProductResponse>>;

internal class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductResponse>>
{
    private readonly IAsyncDocumentSession _documentSession;

    public GetAllProductsQueryHandler(IAsyncDocumentSession documentSession)
    {
        _documentSession = documentSession;
    }
    
    public async Task<List<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _documentSession.Query<Product>()
            .Select(x => new ProductResponse
            {
                Id = x.ProductId,
                Description = x.Description,
                Title = x.Title,
                RemainingCount = x.RemainingCount
            })
            .ToListAsync(cancellationToken);

        return products;
    }
}