using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace cpts483.Models.ViewModels
{
    public class UsersViewModel
    {
        public List<ApplicationUser> Users { get; set; }
        public List<IdentityRole> Roles { get; set; }

        public UsersViewModel()
        {
            Users = new List<ApplicationUser>();
            Roles = new List<IdentityRole>();
        }
    }
}