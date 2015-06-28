using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Blogs.Models
{
    /// <summary>
    /// In order to support automatic DB dropping whenever a change is detected, we have to create a class
    /// derived from DropCreateDatabaseIfModelChanges.  Code wise, it doesn't do very much.
    /// </summary>
    /// 
    public class BlogPosts
    {
        public string post = "";
        public List<string> comments = new List<string>();
    }

    public class ContextModelChangeInitializer : DropCreateDatabaseIfModelChanges<MyDbContext>
    {
        protected override void Seed(MyDbContext context)
        {
            base.Seed(context);//952800

            //on new DB create, add "Joe User" to the DB
            context.People.Add(new PersonModel()
            {
                Email = "Admin@Blogs.com",
                FirstName = "Admin",
                LastName = "Blogs",
                Password = "password",
                isAdmin = true,
                CanWriteArticles = true
            });
            context.SaveChanges();
        }
    }

    public class MyDbContext : DbContext
    {
        private void init()
        {
            //calling this will tell EF to automatically delete the 
            //actual DB whenever a change is detected.  Very handy for debugging, but not so
            //much for production.
            Database.SetInitializer<MyDbContext>(new ContextModelChangeInitializer());
        }
        public MyDbContext()
        {
            init();
            CurrentBlogger = People.FirstOrDefault();
        }

        public MyDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            init();
        }

        //Our DB will have one table, which will be based on the Person class.
        public PersonModel CurrentBlogger = new PersonModel();
        public DbSet<PersonModel> People { get; set; }
        public DbSet<BlogEntry> BlogEntries { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}