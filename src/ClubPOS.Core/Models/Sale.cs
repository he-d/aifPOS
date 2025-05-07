using System;
using System.Collections.Generic;

namespace ClubPOS.Core.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public string ReceiptNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public DateTime SaleDate { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<SaleItem> SaleItems { get; set; }
    }
} 