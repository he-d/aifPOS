using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClubPOS.Core.Models;

namespace ClubPOS.Core.Interfaces
{
    public interface ISaleRepository
    {
        Task<Sale> GetByIdAsync(int id);
        Task<Sale> GetByReceiptNumberAsync(string receiptNumber);
        Task<IEnumerable<Sale>> GetAllAsync();
        Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Sale>> GetByUserIdAsync(int userId);
        Task<Sale> AddAsync(Sale sale);
        Task UpdateAsync(Sale sale);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByReceiptNumberAsync(string receiptNumber);
        Task<decimal> GetTotalSalesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalSalesByUserIdAsync(int userId, DateTime startDate, DateTime endDate);
    }
} 