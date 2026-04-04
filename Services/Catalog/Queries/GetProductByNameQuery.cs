using Catalog.Responses;
using MediatR;

namespace Catalog.Queries
{
    public record GetProductByNameQuery(string name) : IRequest<IList<ProductResponse>>
    {
    }
}
