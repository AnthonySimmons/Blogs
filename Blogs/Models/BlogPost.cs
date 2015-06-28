using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Blogs.Models
{
    public class BlogPost
    {
        [DataType(DataType.PostalCode)]
        public int bloggerId { get; set; }

        [DataType(DataType.MultilineText)]
        public string blog { get; set; }

        [DataType(DataType.MultilineText)]
        public string newComment { get; set; }

        [DataType(DataType.MultilineText)]
        public List<string> comments  = new List<string>();
    }
}