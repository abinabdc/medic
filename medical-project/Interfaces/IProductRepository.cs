using medical_project.Dtos;
using medical_project.Models;

namespace medical_project.Interfaces
{
    public interface IProductRepository
    {
        void Update(Product product);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<ProductDto>> GetAllProductAsync();
        Task<ProductDto> GetAllProductByIdAsync(int Id);
        Task<IEnumerable<ProductDto>> GetProductByStoreId(int Id);
        Task<bool> ProductExists(string name);

    }
}
