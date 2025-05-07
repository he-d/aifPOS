using ClubPOS.Core.Models;
using ClubPOS.Core.Services;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;

namespace ClubPOS.Infrastructure.Services
{
    public class ReceiptPrinterService : IReceiptPrinterService
    {
        private readonly string _printerName;

        public ReceiptPrinterService(string printerName)
        {
            _printerName = printerName;
        }

        public async Task<bool> PrintReceiptAsync(Sale sale)
        {
            if (!sale.Product.PrintReceipt)
            {
                return true;
            }

            try
            {
                var printDocument = new PrintDocument();
                printDocument.PrinterSettings.PrinterName = _printerName;
                printDocument.PrintPage += (sender, e) =>
                {
                    var graphics = e.Graphics;
                    var font = new Font("Courier New", 10);
                    var brush = Brushes.Black;
                    float y = 0;

                    // Print header
                    graphics.DrawString("CLUB POS RECEIPT", font, brush, 0, y);
                    y += font.GetHeight();

                    // Print sale details
                    graphics.DrawString($"Product: {sale.Product.Name}", font, brush, 0, y);
                    y += font.GetHeight();
                    graphics.DrawString($"Quantity: {sale.Quantity}", font, brush, 0, y);
                    y += font.GetHeight();
                    graphics.DrawString($"Price: {sale.Product.Price:C}", font, brush, 0, y);
                    y += font.GetHeight();
                    graphics.DrawString($"Total: {sale.TotalPrice:C}", font, brush, 0, y);
                    y += font.GetHeight();
                    graphics.DrawString($"Date: {sale.CreatedAt:g}", font, brush, 0, y);
                    y += font.GetHeight();
                    graphics.DrawString($"Seller: {sale.User.Username}", font, brush, 0, y);
                    y += font.GetHeight();

                    // Print footer
                    graphics.DrawString("Thank you for your purchase!", font, brush, 0, y);
                };

                printDocument.Print();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> PrintMultipleReceiptsAsync(IEnumerable<Sale> sales)
        {
            foreach (var sale in sales)
            {
                if (!await PrintReceiptAsync(sale))
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> TestPrinterConnectionAsync()
        {
            try
            {
                var printDocument = new PrintDocument();
                printDocument.PrinterSettings.PrinterName = _printerName;
                printDocument.PrintPage += (sender, e) =>
                {
                    var graphics = e.Graphics;
                    var font = new Font("Courier New", 10);
                    var brush = Brushes.Black;
                    graphics.DrawString("Printer Test", font, brush, 0, 0);
                };

                printDocument.Print();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
} 