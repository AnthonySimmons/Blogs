using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cpts483.Models.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FavoriteAnimal { get; set; }
    }
}