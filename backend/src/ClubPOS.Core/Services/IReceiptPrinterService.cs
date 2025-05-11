using ClubPOS.Core.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ClubPOS.Core.Services
{
    public interface IReceiptPrinterService
    {
        Task<string> GenerateReceiptAsync(Sale sale);
        Task<bool> PrintReceiptAsync(string receiptContent);
        Task<bool> TestPrinterConnectionAsync();
        Task<bool> PrintMultipleReceiptsAsync(IEnumerable<Sale> sales);
    }
} 