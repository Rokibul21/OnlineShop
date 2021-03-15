using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OnelineShop.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Order")]
        public int OrderId { get; set; }
        [Required]
        [Display(Name = "Product")]
        public int ProductId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        [ForeignKey("ProductId")]
        public Products product { get; set; }
    }
}
