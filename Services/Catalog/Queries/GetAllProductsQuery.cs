using Catalog.Responses;
using Catalog.Specification;
using MediatR;

namespace Catalog.Queries
{
    public record GetAllProductsQuery(CatalogSpecParams CatalogSpecParams) : IRequest<Pagination<ProductResponse>>
    {
    }
}
