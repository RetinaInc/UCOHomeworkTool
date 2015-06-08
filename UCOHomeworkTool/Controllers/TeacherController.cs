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
using System.Data.Entity.Validation;

namespace UCOHomeworkTool.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            using (var db = new ApplicationDbContext())
            {
                var user = db.Users.Find(userId);
                var userCourses = user.Courses;
                return View(userCourses.ToList());
            }
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
                return PartialView(assignmentModel);
            }
        }
        public ActionResult Course(int id)
        {

            var userId = User.Identity.GetUserId();
            using (var db = new ApplicationDbContext())
            {
                var assignmentList = db.Courses.Find(id).Templates.ToList();
                return View(assignmentList);
            }

        }
        public ActionResult AssignProblems(List<int> probids, int assignmentId)
        {
            if(probids == null)
            {
                probids = new List<int>();
            }
            using (var db = new ApplicationDbContext())
            {
                var assignment = db.Assignments.Find(assignmentId);
                assignment.MakeAssignment(probids, db);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
    }
}