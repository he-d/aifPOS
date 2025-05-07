using ClubPOS.Core.Models;

namespace ClubPOS.Core.Services
{
    public interface ISalesService
    {
        Task<Sale> CreateSaleAsync(Sale sale);
        Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalSalesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetCurrentCashBalanceAsync();
        Task<bool> UpdateCashBalanceAsync(decimal amount);
    }
} 