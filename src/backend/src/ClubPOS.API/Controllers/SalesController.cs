using ClubPOS.Core.Models;
using ClubPOS.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClubPOS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _salesService;

        public SalesController(ISalesService salesService)
        {
            _salesService = salesService;
        }

        [HttpPost]
        public async Task<ActionResult<Sale>> CreateSale(Sale sale)
        {
            var createdSale = await _salesService.CreateSaleAsync(sale);
            return CreatedAtAction(nameof(GetSalesByDateRange), new { startDate = DateTime.UtcNow.Date, endDate = DateTime.UtcNow.Date }, createdSale);
        }

        [HttpGet("by-date")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Sale>>> GetSalesByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var sales = await _salesService.GetSalesByDateRangeAsync(startDate, endDate);
            return Ok(sales);
        }

        [HttpGet("total-by-date")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<decimal>> GetTotalSalesByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var total = await _salesService.GetTotalSalesByDateRangeAsync(startDate, endDate);
            return Ok(total);
        }

        [HttpGet("cash-balance")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<decimal>> GetCurrentCashBalance()
        {
            var balance = await _salesService.GetCurrentCashBalanceAsync();
            return Ok(balance);
        }

        [HttpPut("cash-balance")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateCashBalance([FromQuery] decimal amount)
        {
            var result = await _salesService.UpdateCashBalanceAsync(amount);
            if (!result)
            {
                return BadRequest("Failed to update cash balance");
            }
            return Ok();
        }
    }
} 