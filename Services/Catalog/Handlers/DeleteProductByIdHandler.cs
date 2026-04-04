using Catalog.Commands;
using Catalog.Repositories;
using MediatR;

namespace Catalog.Handlers
{
    public class DeleteProductByIdHandler : IRequestHandler<DeleteProductByIdCommand, bool>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductByIdHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Task<bool> Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
        {
            return _productRepository.DeleteProduct(request.Id);
        }
    }
}
