using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureStore.Models.ViewModels
{
    public class CartInfoDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string UserId { get; set; }

        public CartInfoDTO()
        {

        }
    }
}

