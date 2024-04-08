using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureStore.Models
{
    public class Discount
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "decimal(8,2)")]
        public decimal Percentage { get; set; }
    }
}
