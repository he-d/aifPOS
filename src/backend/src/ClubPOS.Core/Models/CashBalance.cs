using System;

namespace ClubPOS.Core.Models
{
    public class CashBalance
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public DateTime LastUpdated { get; set; }
    }
} 