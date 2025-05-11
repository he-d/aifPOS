using ClubPOS.Core.Models;
using ClubPOS.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClubPOS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PrinterController : ControllerBase
    {
        private readonly IReceiptPrinterService _printerService;

        public PrinterController(IReceiptPrinterService printerService)
        {
            _printerService = printerService;
        }

        [HttpPost("test")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> TestPrinter()
        {
            var result = await _printerService.TestPrinterConnectionAsync();
            if (!result)
            {
                return BadRequest("Failed to connect to printer");
            }
            return Ok();
        }

        [HttpPost("print")]
        public async Task<ActionResult> PrintReceipt([FromBody] Sale sale)
        {
            var receiptContent = await _printerService.GenerateReceiptAsync(sale);
            var result = await _printerService.PrintReceiptAsync(receiptContent);
            if (!result)
            {
                return BadRequest("Failed to print receipt");
            }
            return Ok();
        }

        [HttpPost("print-multiple")]
        public async Task<ActionResult> PrintMultipleReceipts([FromBody] IEnumerable<Sale> sales)
        {
            var result = await _printerService.PrintMultipleReceiptsAsync(sales);
            if (!result)
            {
                return BadRequest("Failed to print receipts");
            }
            return Ok();
        }
    }
} 