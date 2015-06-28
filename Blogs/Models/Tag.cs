using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blogs.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }
        public BlogEntry article { get; set; }
        public int articleId { get; set; }
    }
}