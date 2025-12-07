using DAL.Models;
using PL.DTOS;

namespace BLL.Interface.Services.Abstractions
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> CreateAsync(Product entity);
        Task<Product?> UpdateAsync(int id, Product entity);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Product>> SearchAsync(ProductSearchDTO search);
        Task<decimal?> CalculateFinalPriceAsync(int id);
    }
}
