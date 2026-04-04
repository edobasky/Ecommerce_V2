using Catalog.Commands;
using Catalog.Extension;
using Catalog.Repositories;
using MediatR;

namespace Catalog.Handlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var existing = await _productRepository.GetProduct(request.Id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Product with Id {request.Id} not found");
            }

            var brand = await _productRepository.GetBrandByIdAsync(request.BrandId);
            var type = await _productRepository.GetTypeByTypeIdAsync(request.TypeId);
            if (brand == null || type == null)
            {
                throw new ApplicationException("Invalid Brand or Type specified");
            }
            // Step 2 : Mapper role
            var updateProduct = request.ToUpdateEntity(existing, brand, type);

            // save record
            return await _productRepository.UpdateProduct(updateProduct);
        }
    }
}
