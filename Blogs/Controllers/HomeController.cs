using Blogs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blogs.Controllers
{
    public class HomeController : Controller
    {
        //can be used across all action methods in this class
        public MyDbContext Db;

        /// <summary>
        /// MVC apparently needs a default constructor
        /// </summary>
        public HomeController()
            : this(null)  //note that calling this(null) will also call 
        //code inside HomeController(MyDbContext)
        {

        }

        /// <summary>
        /// Initializes the home controller with the supplied DbContext
        /// </summary>
        /// <param name="someDb"></param>
        public HomeController(MyDbContext someDb = null)
        {
            //If we were given a null context, just use the default context
            if (someDb == null)
            {
                Db = new MyDbContext("MyDbContext");
            }
            else
            {
                //otherwise, use the supplied context
                Db = someDb;
            }
        }

        /// <summary>
        /// Will show a list of all users within the system
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //query using fluent API
            List<PersonModel> people = Db.People
                .Where(p => p.Id > 0)
                .ToList();

            //query using LINQ (same as the above fluent API method)
            List<PersonModel> people2 = (
                                    from person in Db.People
                                    where person.Id > 0
                                    select person
                                   ).ToList();

            //return the "View" to MVC.  By default, MVC will look for a view titled
            //"index.cshtml" inside the "Views/Home" folder.  Note that we're also sending
            //a list of People objects for the view to process.
            return View(people);
        }

        public ActionResult NewBlogSubmit(string text, int id)
        {
            PersonModel blogger = Db.People.Where(bl => bl.Id == id).FirstOrDefault();
            if (blogger != null)
            {
                BlogPost item = new BlogPost();
                item.blog = text;
                blogger.blogPosts.Add(item);
            }
            //Db.CurrentBlogger = blogger;
            Db.SaveChanges();
            return View("MyBlogs", Db.CurrentBlogger);
        }

        public ActionResult NewBlog(int id)
        {
            PersonModel blogger = Db.People.Where(bl => bl.Id == id).FirstOrDefault();
            if (blogger != null)
            {
                blogger.blogPosts.Add(new BlogPost());
               // Db.CurrentBlogger = blogger;
            }//
            return View("NewBlog", new PersonModel());
        }

        [HttpPost, ActionName("NewBlog")]
        public ActionResult NewBlog(PersonModel model)
        {
            PersonModel person = Db.People.Where(p => p.Id == model.Id).FirstOrDefault();
            if (person != null)
            {
                BlogPost bp = new BlogPost();
                bp.blog = model.newBlog;
                person.blogPosts.Add(bp);
                //db.Blogs.Add(bp);
                model.newBlog = "";
                //Db.CurrentBlogger = person;
            }

            //person.blogPosts = db.Blogs.Where(b => b.bloggerId == person.Id).ToList();
            Db.SaveChanges();
            return View("MyBlogs", Db.CurrentBlogger);
        }

        

        public ActionResult DeleteAccount(int id)
        {
            PersonModel p = Db.People.Where(c => c.Id == id).FirstOrDefault();
            if (p != null)
            {
                Db.Entry(p).State = System.Data.Entity.EntityState.Deleted;
                Db.SaveChanges();
            }
            return RedirectToAction("Login");
        }

        [HttpPost, ActionName("NewComment")]
        public ActionResult NewComment(PersonModel model)
        {
            PersonModel person = Db.People.Where(p => p.Id == model.Id).FirstOrDefault();
            if (person != null)
            {
                BlogPost bp = new BlogPost();
                bp.comments.Add(model.newComment);
                person.blogPosts.Add(bp);
                model.newComment = "";
                Db.SaveChanges();
                return View("MyBlogs", new { model = person });
            }
            else
            {
                return View("MyBlogs");
            }
            
        }


        public ActionResult Logout()
        {
            Session["isadmin"] = false;
            Session["login"] = false;
            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult Login(PersonModel model)
        {
            int uId = 0;
            bool found = false;
            Session["userid"] = null;
            Session["isadmin"] = false;
            Session["canwrite"] = false;

            foreach (PersonModel p in Db.People)
            {
                if (model.Email == p.Email && model.Password == p.Password)
                {
                    uId = p.Id;
                    Session["userid"] = p.Id;
                    Session["name"] = p.FirstName + " " + p.LastName;
                    Session["isadmin"] = p.isAdmin;
                    Session["login"] = true;
                    Session["canwrite"] = p.CanWriteArticles;
                    Db.CurrentBlogger = p;
                    found = true;
                    //break;

                }
            }
            if (!found)
            {
                foreach (string key in ModelState.Keys)
                {
                    ModelState[key].Errors.Clear();
                }

                ModelState.AddModelError("", "Invalid User Name / Password");
                return View("Login");
            }
            Db.SaveChanges();
            //return RedirectToAction("Login");
            //return View("MyBlogs", new { model = db.CurrentBlogger });
            return RedirectToAction("BlogIndex", "Blog");
            
        }


        /// <summary>
        /// View method to allow the creation of a new Person in the DB
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            Session["isadmin"] = false;
            Session["login"] = false;
            //return the view with a newly created (and empty) Person object.
            return View("Create", new PersonModel());
        }


        /// <summary>
        /// Postback version of the Create action.  Note that this method will only be
        /// called on HTTP POST messages (as indicated by the [HttpPost] attribute.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(PersonModel model)
        {
            //MVC uses ModelState to track the correctness of the supplied Person "model."
            //If the model is valid, it means that MVC didn't detect any errors as defined
            //in the attributes in the Person class.
            if (ModelState.IsValid)
            {
                //Add the person to the DB.  This queues them for insertion but doesn't
                //actually insert the value into the DB.  For that, we need the next command.
                //model.CanWriteArticles = true;
                Db.People.Add(model);

                //tell the DB to save any queued changes.
                Db.SaveChanges();

                //Once we're done, redirect to the Index action of HomeController.
                return RedirectToAction("Login");
            }
            else
            {
                ModelState.AddModelError("", "One or more issues were found with your submission. Please try again.");
            }
            //If we got here, it means that the model's state is invalid.  Simply return
            //to the create page and display any errors.
            return View("Login", model);
        }

        /// <summary>
        /// This action allows the user to edit a given Person
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id = -1)
        {
            if (id < 0)
            {
                id = (int)Session["userid"];
            }
            //Find the person in the DB.  Use the supplied "id" integer as a reference.
            PersonModel somePerson = Db.People
                .Where(p => p.Id == id)     //this line says to find the person whose ID matches our parameter
                .FirstOrDefault();          //FirstOrDefault() returns either a singluar Person object or NULL

            //If we got NULL, it must mean that we were supplied an incorrect ID.  
            //In this case, redirect to HomeController's Index action.
            if (somePerson == null)
            {
                return RedirectToAction("Index");
            }

            //If we're here, then we must have a valid person.  Send to the "Create" view because
            //create and edit are kind of the same thing.  The 2nd parameter is the model that
            //we will be sending to the Create view.
            return View("Create", somePerson);
        }

        /// <summary>
        /// Postback version of the Edit action.  Will be called when the browser sends us information
        /// back to the server.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(PersonModel model)
        {
            //again, check modelstate to make sure everything went okay
            if (ModelState.IsValid)
            {
                //Because the item already exists in the DB, we want to tell EF that
                //one of its models has been changed.  We use this somewhat strange syntax to
                //accomplish this task.
                Db.Entry(model).State = System.Data.Entity.EntityState.Modified;

                //Again, the above command adds the request to a queue.  To execute the queue,
                //we need to call SaveChanges()
                Db.SaveChanges();

                //when complete, redirect to Index
                return RedirectToAction("Index");
            }

            //Things must've went bad, so send back to the Create view.
            return View("Create", model);
        }
    }
}