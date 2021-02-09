using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnelineShop.Models
{
    public class SpacialTag
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name ="Special Tag")]
        public string Name { get; set; }
        public virtual List<Products> products { get; set; }
    }
}
