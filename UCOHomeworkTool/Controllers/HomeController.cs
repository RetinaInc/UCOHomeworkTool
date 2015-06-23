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
            if(User.IsInRole("Teacher"))
            {
                return RedirectToAction("Index", "Teacher");
            }
            var userId = User.Identity.GetUserId();
            using (var db = new ApplicationDbContext())
            {
                var user = db.Students.Find(userId);
                var userCourses = user.CoursesTaking;
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
                foreach (var prob in assignmentModel.Problems)
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
            ViewBag.CourseId = id;
            using (var db = new ApplicationDbContext())
            {
                var assignmentList = db.Assignments.Where(x => x.Course.Id == id && x.Student.Id == userId).OrderBy(x => x.AssignmentNumber).ToList();
                return View(assignmentList);
            }

        }
        public ActionResult DiagramImage(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var imageData = db.ProblemDiagrams.Where(d => d.ProblemId == id).Select(d => d.ImageContent).FirstOrDefault();
                return File(imageData, "image/jpg");
            }

        }
        public JsonResult CheckResponse(List<JsonResponseObject> responses, int probId,int assignId)
        {
            using (var db = new ApplicationDbContext())
            {
                var p = db.Problems.Find(probId);
                var assignment = db.Assignments.Find(assignId);
                var jsonReply = new List<object>();
                bool allCorrect = true;
                if (p.TrysRemaining > 0)
                {
                    foreach (var response in responses)
                    {
                        Response r = db.Responses.Find(response.id);
                        switch (p.TrysRemaining)
                        {
                            case 5:
                                r.FirstAttempt = response.value;
                                break;
                            case 4:
                                r.SecondAttempt = response.value;
                                break;
                            case 3:
                                r.ThirdAttempt = response.value;
                                break;
                            case 2:
                                r.FourthAttempt= response.value;
                                break;
                            case 1:
                                r.FifthAttempt= response.value;
                                break;
                        }
                        var isCorrect = r.Expected.Equals(response.value);
                        allCorrect = allCorrect && isCorrect;
                        jsonReply.Add(new { id = response.id, correct = isCorrect, trysLeft = p.TrysRemaining - 1 });
                    }
                    p.TrysRemaining -= 1;
                    if(allCorrect || p.TrysRemaining == 0)
                    {
                        assignment.GradeAssignment();
                    }
                    db.SaveChanges();
                }
                return Json(jsonReply, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Grades(int id)
        {
            var userId = User.Identity.GetUserId();
            ViewBag.CourseId = id;
            using(var db = new ApplicationDbContext())
            {
                ViewBag.CourseName = db.Courses.Find(id).Name;
                var assignments = db.Assignments.Where(a => a.Student.Id == userId && a.Course.Id == id).ToList();
                return View(assignments);
            }
        }
        public class JsonResponseObject
        {
            public int id { get; set; }
            public double value { get; set; }
        }
    }
}