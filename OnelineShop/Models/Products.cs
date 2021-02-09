using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnelineShop.Models
{
    public class Products
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string Image { get; set; }
        [Required]
        [Display(Name = "Product Color")]
        public string ProductColor { get; set; }
        [Required]
        [Display(Name = "Avilable")]
        public bool IsAvilable { get; set; }

        [Required]
        [Display(Name = "Product Type")]
        public int ProductTypes { get; set; }
        [ForeignKey("ProductTypes")]
        public virtual ProductTypes ProductType { get; set; }

        [Required]
        [Display(Name = "Spacial Tag")]
        public int SpacilTag { get; set; }
        [ForeignKey("SpacilTag")]
        public virtual SpacialTag SpacialTag { get; set; }

    }
}
