using Catalog.Entities;

namespace Catalog.Repositories
{
    public interface ITypeRepository
    {
        Task<IEnumerable<ProductType>> GetAllTyoes();
        Task<ProductType> GetByIdAsync(string id);
    }
}
