using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Friello.ViewModels
{
    public class RoleVM
    {
        public string Fullname { get; set; }
        public List<IdentityRole> roles { get; set; }
        public IList<string> userRoles { get; set; }
        public string userId { get; set; }
    }
}
