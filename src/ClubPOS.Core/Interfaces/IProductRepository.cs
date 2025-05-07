using System.Collections.Generic;
using System.Threading.Tasks;
using ClubPOS.Core.Models;

namespace ClubPOS.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(int id);
        Task<Product> GetByBarcodeAsync(string barcode);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetByCategoryAsync(string category);
        Task<IEnumerable<Product>> GetLowStockAsync();
        Task<Product> AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByBarcodeAsync(string barcode);
        Task UpdateStockAsync(int id, int quantity);
    }
} 