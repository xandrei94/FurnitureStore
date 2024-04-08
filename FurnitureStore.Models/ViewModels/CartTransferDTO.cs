using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureStore.Models.ViewModels
{
    public class CartTransferDTO

    {
        public int Id { get; set; }
        public ProductDTO Product { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double TotalPrice { get; set; }
        public int ShoppingCartId { get; set; }
        public string Status { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string ProductCode { get; set; }
        public string Supplier { get; set; }
        public string Manufacturer { get; set; }
        public double Price { get; set; }
        public bool IsAvailableForPurchase { get; set; }
    }
}

