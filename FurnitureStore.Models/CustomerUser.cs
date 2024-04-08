using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureStore.Models
{
    public class CustomerUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public string? Adress { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public int? DiscountId { get; set; }
        [ForeignKey("DiscountId")]
        public Discount? Discount { get; set; }
    }
}
