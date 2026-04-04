using Catalog.Extension;
using Catalog.Queries;
using Catalog.Repositories;
using Catalog.Responses;
using Catalog.Specification;
using MediatR;

namespace Catalog.Handlers
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, Pagination<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductsHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Pagination<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var productList = await _productRepository.GetProducts(request.CatalogSpecParams);
            var producctResponseList = productList.ToResponse();
            return producctResponseList;
        }
    }
}
