using ClubPOS.Core.Models;

namespace ClubPOS.Core.Services
{
    public interface IReceiptPrinterService
    {
        Task<bool> PrintReceiptAsync(Sale sale);
        Task<bool> PrintMultipleReceiptsAsync(IEnumerable<Sale> sales);
        Task<bool> TestPrinterConnectionAsync();
    }
} 