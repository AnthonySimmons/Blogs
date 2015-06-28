using Blogs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blogs.Models.ViewModels;

namespace Blogs.Controllers
{
    public class BlogController : HomeController
    {
        //MyDbContext Db = new MyDbContext();
        //
        // GET: /Wednesday/
        public ActionResult BlogIndex(int id = -1)
        {
            if (id <= 0)
            {
                //return RedirectToAction("CreateBlog");
            }
            BlogEntryViewModel vm = new BlogEntryViewModel();
            vm.CurrentBlogEntry = Db.BlogEntries.Where(b => b.Id == id).FirstOrDefault();
            vm.AllEntries = Db.BlogEntries.ToList();
            vm.Comments = Db.Comments.ToList();
            vm.AllTags = Db.Tags.ToList();
            
            //int userid = (int)Session["userid"];
            //Db.CurrentBlogger = Db.People.Where(p => p.Id == userid).FirstOrDefault();
            if (vm.CurrentBlogEntry != null)
            {
                int authId;
                Int32.TryParse(vm.CurrentBlogEntry.AuthorId, out authId);
                vm.CurrentUser = Db.People.Where(p => p.Id == authId).FirstOrDefault();
                vm.CurrentBlogEntry.ArticleComments = Db.Comments.Where(p => p.blogId == vm.CurrentBlogEntry.Id).ToList();
                vm.CurrentBlogEntry.ArticleTags = Db.Tags.Where(p => p.articleId == vm.CurrentBlogEntry.Id).ToList();
            }
            
            return View(vm);
        }



        public ActionResult BlogEntry()
        {
            BlogEntry entry = Db.BlogEntries.Where(b => b.Id == 0).FirstOrDefault();
            return View("_BlogEntry", entry);
        }

        public ActionResult BlogEntryAsync(int id)
        {
            BlogEntry entry = Db.BlogEntries.Where(b => b.Id == id).FirstOrDefault();
            return View("_BlogEntry", entry);
        }



        public ActionResult DeleteComment(int id = -1, int blogId = -1)
        {
            BlogEntry blog = Db.BlogEntries.Where(b => b.Id == blogId).FirstOrDefault();
            Comment entry = Db.Comments.Where(b => b.id == id).FirstOrDefault();
            if(blog != null)
            {
                int authId;
                string sessionId = Session["userid"].ToString();
                Int32.TryParse(blog.AuthorId, out authId);
                if ((string)sessionId.ToString() != (string)blog.AuthorId)
                {
                    return RedirectToAction("BlogIndex", new { id = blogId });        
                }
            }
            if (entry != null)
            {
                Db.Entry(entry).State = System.Data.Entity.EntityState.Deleted;
                Db.SaveChanges();
            }
            return RedirectToAction("BlogIndex", new { id = blogId });
        }


        public ActionResult EditBlog(int id = -1)
        {
            BlogEntry entry = Db.BlogEntries.Where(b => b.Id == id).FirstOrDefault();
            if (entry != null)
            {
                if (entry.AuthorId != Session["userid"].ToString())
                {
                    return RedirectToAction("BlogIndex", new { id = id });
                }
            }
            return View("CreateBlog", entry);
        }

        [HttpPost]
        public ActionResult EditBlog(BlogEntry model)
        {
            BlogEntry entry = Db.BlogEntries.Where(b => b.Id == model.Id).FirstOrDefault();

            if (entry != null)
            {
                entry.LastEdited = DateTime.Now;

                entry.Title = model.Title;
                entry.Content = model.Content;
                entry.isAcceptingComments = model.isAcceptingComments;
                entry.isVisible = model.isVisible;
                

                Db.Entry(entry).State = System.Data.Entity.EntityState.Modified;
                Db.SaveChanges();
            }
            return RedirectToAction("BlogIndex", new { id = model.Id });
        }

        public ActionResult DeleteBlog(int id = -1)
        {
            BlogEntry entry = Db.BlogEntries.Where(b => b.Id == id).FirstOrDefault();

            if (entry != null)
            {
                if (entry.AuthorId == Session["userid"].ToString())
                {
                    Db.Entry(entry).State = System.Data.Entity.EntityState.Deleted;
                    Db.SaveChanges();
                }
            }
            return RedirectToAction("BlogIndex");
            //return RedirectToAction("Create");
        }

        public ActionResult CreateBlog()
        {
            return View(new BlogEntry());
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult CreateBlog(BlogEntry model)
        {
            model.DatePosted = DateTime.Now;
            model.LastEdited = DateTime.Now;
            model.Author = Session["name"].ToString();//Db.CurrentBlogger.FirstName + " " + Db.CurrentBlogger.LastName;
            model.AuthorId = Session["userid"].ToString();
            Db.BlogEntries.Add(model);

            Db.SaveChanges();
            return RedirectToAction("BlogIndex", new { id = model.Id });
        }

        public ActionResult CreateTag(int modelId)
        {
            BlogEntry entry = Db.BlogEntries.Where(m => m.Id == modelId).FirstOrDefault();
            Tag tag = new Tag();
            tag.articleId = modelId;
            if (entry != null)
            {
                tag.article = entry;
            }

            return View(tag);
        }

        [HttpPost]
        public ActionResult CreateTag(Tag tag)
        {
            Db.Tags.Add(tag);
            Db.SaveChanges();
            return RedirectToAction("BlogIndex", new { id = tag.articleId });
        }

        public ActionResult CreateComment(int modelId)
        {
            Comment com = new Comment();
            com.blogId = modelId;
            return View(com);
        }

        [HttpPost]
        public ActionResult CreateComment(Comment com)
        {
            //com.blogId = blogId;
            com.authorId = Session["userid"].ToString();
            com.author = Session["name"].ToString();
            com.datePosted = DateTime.Now;
            Db.Comments.Add(com);
            Db.SaveChanges();
            return RedirectToAction("BlogIndex", new { id = com.blogId });
        }
    }
}