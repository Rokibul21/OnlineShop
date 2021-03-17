using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnelineShop.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FristName { get; set; }
        public string LastName { get; set; }
    }
}
