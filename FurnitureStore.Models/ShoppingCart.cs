using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureStore.Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public DateTime UpdatedAt { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }

    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double Price { get; set; }
        public double TotalPrice { get; set; }
        public int ShoppingCartId { get; set; }
        [Required]
        [EnumDataType(typeof(CartItemStatus))]
        public string Status { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public enum CartItemStatus
    {
        Added,
        Purchased,
        Removed
    }
}
