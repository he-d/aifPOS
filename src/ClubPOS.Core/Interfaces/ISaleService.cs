using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClubPOS.Core.Models;

namespace ClubPOS.Core.Interfaces
{
    public interface ISaleService
    {
        Task<Sale> GetByIdAsync(int id);
        Task<Sale> GetByReceiptNumberAsync(string receiptNumber);
        Task<IEnumerable<Sale>> GetAllAsync();
        Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Sale>> GetByUserIdAsync(int userId);
        Task<Sale> CreateSaleAsync(Sale sale, IEnumerable<SaleItem> items);
        Task UpdateSaleAsync(Sale sale);
        Task DeleteSaleAsync(int id);
        Task<decimal> GetTotalSalesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalSalesByUserIdAsync(int userId, DateTime startDate, DateTime endDate);
        Task<string> GenerateReceiptNumberAsync();
        Task<bool> ValidateSaleItemsAsync(IEnumerable<SaleItem> items);
    }
} 