using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureStore.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string ProductCode { get; set; }
        [Required]
        public string Supplier { get; set; }
        [Required]
        public string Manufacturer { get; set; }
        public int Stock { get; set; }
        [Display(Name = "List Price")]
        [Range(1, 10000)]
        public double ListPrice { get; set; }
        [Display(Name = "Price")]
        [Range(0, 10000)]
        public double Price { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
        [Display(Name = "Available")]
        public bool IsAvailableForPurchase { get; set; }
    }
}
