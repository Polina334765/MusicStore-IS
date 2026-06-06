using System.Collections.Generic;

namespace MusicStore.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime SaleDate { get; set; } = DateTime.Now;
        public List<SaleItem> Items { get; set; } = new();
        public decimal TotalAmount => Items.Sum(item => item.Quantity * item.Product.Price);
    }

    public class SaleItem
    {
        public Product Product { get; set; } = null!;
        public int Quantity { get; set; }
    }
}