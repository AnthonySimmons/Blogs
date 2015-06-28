using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blogs.Models
{
    public class PersonModel
    {

        [DataType(DataType.MultilineText)]
        public string newBlog { get; set; }


        [DataType(DataType.MultilineText)]
        public List<BlogPost> blogPosts = new List<BlogPost>();


        [DataType(DataType.MultilineText)]
        public string newComment { get; set; }
        /// <summary>
        /// All EF classes need a key for DB storage.  Keys need to be integers.
        /// We use the [Key] tag to tell EF which property we want to use as the DB key.
        /// </summary>
        [Key]
        public int Id { get; set; }

        //Specifying [Required] tells EF that the field cannot be empty.  
        [Required(ErrorMessage = "Please enter your first name.")]

        //[Display] is for making things pretty in MVC views
        [Display(Name = "First Name:")]

        //[MaxLength] tells EF that the corresponding DB field should only be
        //50 characters (NVARCHAR(50) vs NVARCHAR(MAX))
        [MaxLength(50, ErrorMessage = "Name is too long (50 chars max).")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name.")]
        [Display(Name = "Last Name:")]
        public string LastName { get; set; }

        //we can even specify a data type using attributes!
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please enter your email address.")]
        [Display(Name = "Email Address:")]
        public string Email { get; set; }


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter your password.")]
        [Display(Name = "Password:")]
        public string Password { get; set; }

        [Display(Name = "Write Articles")]
        public bool CanWriteArticles { get; set; }

        [Display(Name = "Administrator")]
        public bool isAdmin { get; set; }
    }
}