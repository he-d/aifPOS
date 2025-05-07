using ClubPOS.Core.Models;
using ClubPOS.Core.Services;
using ClubPOS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClubPOS.Infrastructure.Services
{
    public class SalesService : ISalesService
    {
        private readonly ApplicationDbContext _context;

        public SalesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Sale> CreateSaleAsync(Sale sale)
        {
            sale.CreatedAt = DateTime.UtcNow;
            _context.Sales.Add(sale);

            // Update cash balance
            var cashBalance = await _context.CashBalance.FirstOrDefaultAsync();
            if (cashBalance == null)
            {
                cashBalance = new CashBalance { Balance = 0 };
                _context.CashBalance.Add(cashBalance);
            }
            cashBalance.Balance += sale.TotalPrice;
            cashBalance.LastUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return sale;
        }

        public async Task<IEnumerable<Sale>> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Sales
                .Include(s => s.Product)
                .Include(s => s.User)
                .Where(s => s.CreatedAt >= startDate && s.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalSalesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Sales
                .Where(s => s.CreatedAt >= startDate && s.CreatedAt <= endDate)
                .SumAsync(s => s.TotalPrice);
        }

        public async Task<decimal> GetCurrentCashBalanceAsync()
        {
            var cashBalance = await _context.CashBalance.FirstOrDefaultAsync();
            return cashBalance?.Balance ?? 0;
        }

        public async Task<bool> UpdateCashBalanceAsync(decimal amount)
        {
            var cashBalance = await _context.CashBalance.FirstOrDefaultAsync();
            if (cashBalance == null)
            {
                cashBalance = new CashBalance { Balance = 0 };
                _context.CashBalance.Add(cashBalance);
            }
            cashBalance.Balance = amount;
            cashBalance.LastUpdated = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 