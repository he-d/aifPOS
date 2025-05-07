using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClubPOS.Core.Interfaces;
using ClubPOS.Core.Models;
using ClubPOS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClubPOS.Infrastructure.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly ApplicationDbContext _context;

        public SaleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Sale> GetByIdAsync(int id)
        {
            return await _context.Sales
                .Include(s => s.User)
                .Include(s => s.SaleItems)
                    .ThenInclude(si => si.Product)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Sale> GetByReceiptNumberAsync(string receiptNumber)
        {
            return await _context.Sales
                .Include(s => s.User)
                .Include(s => s.SaleItems)
                    .ThenInclude(si => si.Product)
                .FirstOrDefaultAsync(s => s.ReceiptNumber == receiptNumber);
        }

        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            return await _context.Sales
                .Include(s => s.User)
                .Include(s => s.SaleItems)
                    .ThenInclude(si => si.Product)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Sales
                .Include(s => s.User)
                .Include(s => s.SaleItems)
                    .ThenInclude(si => si.Product)
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sale>> GetByUserIdAsync(int userId)
        {
            return await _context.Sales
                .Include(s => s.User)
                .Include(s => s.SaleItems)
                    .ThenInclude(si => si.Product)
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }

        public async Task<Sale> AddAsync(Sale sale)
        {
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();
            return sale;
        }

        public async Task UpdateAsync(Sale sale)
        {
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var sale = await _context.Sales.FindAsync(id);
            if (sale != null)
            {
                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Sales.AnyAsync(s => s.Id == id);
        }

        public async Task<bool> ExistsByReceiptNumberAsync(string receiptNumber)
        {
            return await _context.Sales.AnyAsync(s => s.ReceiptNumber == receiptNumber);
        }

        public async Task<decimal> GetTotalSalesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Sales
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
                .SumAsync(s => s.TotalAmount);
        }

        public async Task<decimal> GetTotalSalesByUserIdAsync(int userId, DateTime startDate, DateTime endDate)
        {
            return await _context.Sales
                .Where(s => s.UserId == userId && s.SaleDate >= startDate && s.SaleDate <= endDate)
                .SumAsync(s => s.TotalAmount);
        }
    }
} 