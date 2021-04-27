using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnelineShop.Areas.Admin.Model
{
    public class UserRoleMapping
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
    }
}
