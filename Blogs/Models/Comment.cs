using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blogs.Models
{
    public class Comment
    {
        public string author { get; set; }
        public string authorId { get; set; }
        public string content { get; set; }
        public DateTime datePosted { get; set; }
        public int blogId { get; set; }
        [Key]
        public int id { get; set; }

        public Comment()
        {
            author = "";
            authorId = "";
            content = "";
            datePosted = DateTime.Now;
            id = 0;
        }
    }
}