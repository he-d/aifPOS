using ClubPOS.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClubPOS.Core.Services
{
    public interface ISalesService
    {
        Task<IEnumerable<Sale>> GetAllSalesAsync();
        Task<Sale> GetSaleByIdAsync(int id);
        Task<Sale> CreateSaleAsync(Sale sale);
        Task<bool> UpdateSaleAsync(Sale sale);
        Task<bool> DeleteSaleAsync(int id);
        Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalSalesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetCurrentCashBalanceAsync();
        Task<bool> UpdateCashBalanceAsync(decimal amount);
    }
} 