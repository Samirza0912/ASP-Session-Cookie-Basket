using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Friello.Models
{
    public class AppUser:IdentityUser
    {
        public string Fullname { get; set; }
        public DateTime UserCreateTime { get; set; }
        public DateTime ConfirmMailTime { get; set; }
        public List<Sale> MyProperty { get; set; }
    }
}
