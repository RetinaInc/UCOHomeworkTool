using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UCOHomeworkTool.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Migrations;
using UCOHomeworkTool.Migrations;

namespace UCOHomeworkTool.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            using(var db = new ApplicationDbContext())
            {
                var user = db.Users.Find(userId);
                var userCourses = user.Courses;
                return View(userCourses.ToList());
            }
        }
        public void DebugDB()
        {
            var configuration = new Configuration();
            var migrator = new DbMigrator(configuration);
            migrator.Update();
        }
        public ActionResult Assignment(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var assignmentModel = db.Assignments.Include("Problems").SingleOrDefault(x => x.Id == id);
                var problemsModel = assignmentModel.Problems;
                foreach (var prob in problemsModel)
                {
                    db.Entry(prob).Collection("Givens").Load();
                    db.Entry(prob).Collection("Responses").Load();
                }
                return PartialView(problemsModel.ToList());
            }
        }
        public ActionResult Course(int id)
        {

            var userId = User.Identity.GetUserId();
            using (var db = new ApplicationDbContext())
            {
                var assignmentList = db.Assignments.Where(x => x.Course.Id == id && x.Student.Id == userId).ToList();
                return View(assignmentList);
            }

        }
    }
}