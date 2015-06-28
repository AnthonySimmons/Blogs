using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blogs.Models
{
    public class BlogEntry
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        [Display(Name = "")]
        public List<Comment> ArticleComments { get; set; }
        public List<Tag> ArticleTags { get; set; }

        [Display(Name = "Visible")]
        public bool isVisible { get; set; }
        public DateTime LastEdited { get; set; }

        
        [Display(Name = "Accept Comments")]
        public bool isAcceptingComments { get; set; }
        public DateTime DatePosted { get; set; }
        public string Author { get; set; }
        public string AuthorId { get; set; }

        public BlogEntry()
        {
            Id = 0;
            Title = "";
            Content = "";
            ArticleComments = new List<Comment>();
            ArticleTags = new List<Tag>();
            isVisible = true;
            LastEdited = DateTime.Now;
            isAcceptingComments = true;
            DatePosted = DateTime.Now;
            Author = "";
            AuthorId = "";
         
        }

    }
}