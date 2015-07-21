using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UCOHomeworkTool.Models;
using PagedList;

namespace UCOHomeworkTool.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ApplicationUsers
        public ActionResult Index()
        {
            int pageSize = 5;
            int pageNumber = 1;
            return View(db.Users.ToList().OrderBy(user => user.LastName).ToPagedList(pageNumber,pageSize));
        }
        public PartialViewResult UserTablePage(int page)
        {
            int pageSize = 5;
            return PartialView("_UserTable", db.Users.OrderBy(u => u.LastName).ToPagedList(page,pageSize));
        }
        // GET: ApplicationUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            var editUser = new EditUserViewModel(applicationUser);
            using (var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db)))
            {
                if (um.IsInRole(applicationUser.Id, "Teacher"))
                {
                    editUser.IsTeacher = true;
                }
                if (um.IsInRole(applicationUser.Id, "Admin"))
                {
                    editUser.IsAdmin = true;
                }
            }
            return View(editUser);
        }

        // GET: ApplicationUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApplicationUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EditUserViewModel user)
        {
            if (ModelState.IsValid)
            {
                var hasher = new PasswordHasher();
                ApplicationUser appUser;
                if (user.IsTeacher)
                {
                    appUser = new Teacher
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.UserName,
                        PasswordHash = hasher.HashPassword(user.Password),
                        SecurityStamp = Guid.NewGuid().ToString(),
                    };
                }
                else if (user.IsAdmin)
                {
                    appUser = new ApplicationUser
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.UserName,
                        PasswordHash = hasher.HashPassword(user.Password),
                        SecurityStamp = Guid.NewGuid().ToString(),
                    };

                }
                else
                {
                    appUser = new Student
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.UserName,
                        PasswordHash = hasher.HashPassword(user.Password),
                        SecurityStamp = Guid.NewGuid().ToString(),

                    };
                }
                db.Users.Add(appUser);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    if (e.EntityValidationErrors.FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage.Contains("taken"))
                    {

                        ModelState.AddModelError("Username", "The username " + user.UserName + " is already in use. Try again with a different username.");
                        return View(user);
                    }
                }
                using (var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db)))
                {
                    if (user.IsTeacher)
                    {
                        um.AddToRole(appUser.Id, "Teacher");
                        um.Update(appUser);

                    }
                    if (user.IsAdmin)
                    {
                        um.AddToRole(appUser.Id, "Admin");
                        um.Update(appUser);
                    }

                }
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: ApplicationUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            var editUser = new EditUserViewModel
            {
                Id = applicationUser.Id,
                LastName = applicationUser.LastName,
                FirstName = applicationUser.FirstName,
                UserName = applicationUser.UserName,
            };
            using (var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db)))
            {
                if (um.IsInRole(applicationUser.Id, "Admin"))
                {
                    editUser.IsAdmin = true;
                }
                if (um.IsInRole(applicationUser.Id, "Teacher"))
                {
                    editUser.IsTeacher = true;
                }
            }
            return View(editUser);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditUserViewModel editUser)
        {
            if (ModelState.IsValid)
            {
                var dbUser = db.Users.Find(editUser.Id);
                dbUser.FirstName = editUser.FirstName;
                dbUser.LastName = editUser.LastName;
                dbUser.UserName = editUser.UserName;
                if (editUser.Password != null)
                {
                    var hasher = new PasswordHasher();
                    dbUser.PasswordHash = hasher.HashPassword(editUser.Password);
                }
                if (editUser.IsTeacher)
                {
                    using (var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db)))
                    {
                        if (editUser.IsAdmin && !um.IsInRole(dbUser.Id, "Admin"))
                        {
                            um.AddToRole(dbUser.Id, "Admin");
                        }
                        else if (!editUser.IsAdmin && um.IsInRole(dbUser.Id, "Admin"))
                        {
                            um.RemoveFromRole(dbUser.Id, "Admin");
                        }
                    }
                }
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    if (e.EntityValidationErrors.FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage.Contains("taken"))
                    {
                        ModelState.AddModelError("Username", "The username " + editUser.UserName + " is already in use. Try again with a different username.");
                        return View(editUser);
                    }

                }
                catch (Exception e)
                {
                    if (e.InnerException.InnerException.ToString().Contains("Cannot insert duplicate"))
                    {
                        ModelState.AddModelError("Username", "The username " + editUser.UserName + " is already in use. Try again with a different username.");
                        return View(editUser);
                    }
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(editUser);
        }

        // GET: ApplicationUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var applicationUser = db.Users.Find(id);
            //check to see if any assignments have been made for this user if they are a student and if so, delete them
            var assignmentsToRemove = db.Assignments.Where(a => a.Student.Id == applicationUser.Id).ToList();
            db.Assignments.RemoveRange(assignmentsToRemove);
            //remove course associations if this user is a teacher
            db.Courses.Where(c => c.Teacher.Id == applicationUser.Id).ToList().ForEach(c => c.Teacher = null);
            db.Users.Remove(applicationUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
