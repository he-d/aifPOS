using System;
using System.Collections.Generic;

namespace ClubPOS.Core.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Barcode { get; set; }
        public string Category { get; set; }
        public int StockQuantity { get; set; }
        public int MinimumStockLevel { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public virtual ICollection<SaleItem> SaleItems { get; set; }
    }
} 