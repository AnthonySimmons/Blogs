using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blogs.Models.ViewModels
{
    public class BlogEntryViewModel
    {
        public List<BlogEntry> AllEntries { get; set; }
        public BlogEntry CurrentBlogEntry { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Tag> AllTags { get; set; }
        public PersonModel CurrentUser { get; set; }
    }

}