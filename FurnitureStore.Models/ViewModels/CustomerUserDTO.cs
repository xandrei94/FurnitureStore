using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureStore.Models.ViewModels
{
    public class CustomerUserDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? DiscountId { get; set; }
        public DiscountDTO? Discount { get; set; }

        public CustomerUserDTO(CustomerUser customerUser)
        {
            Id = customerUser.Id;
            Name = customerUser.Name;
            UserName = customerUser.UserName;
            Email = customerUser.Email;
            PhoneNumber = customerUser.PhoneNumber;
            DiscountId = customerUser.DiscountId;
            Discount = new DiscountDTO
            {
                Name = customerUser.Discount?.Name, // Ensure Discount is not null
                Percentage = customerUser.Discount?.Percentage ?? 0 // Ensure Discount is not null
            };
        }

        public class DiscountDTO
        {
            public string? Name { get; set; }
            public decimal Percentage { get; set; }
        }
    }
}
