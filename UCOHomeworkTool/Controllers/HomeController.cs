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
        public ActionResult DiagramImage(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var imageData = db.ProblemDiagrams.Where(d => d.ProblemId == id).Select(d => d.ImageContent).FirstOrDefault();
                return File(imageData, "image/jpg");
            }

        }
        public JsonResult CheckResponse(List<JsonResponseObject> responses, int probId)
        {
            using (var db = new ApplicationDbContext())
            {
                var p = db.Problems.Find(probId);
                var jsonReply = new List<object>();
                if (p.TrysRemaining > 0)
                {
                    foreach (var response in responses)
                    {
                        Response r = db.Responses.Find(response.id);
                        switch (p.TrysRemaining)
                        {
                            case 3:
                                r.FirstAttempt = response.value;
                                break;
                            case 2:
                                r.SecondAttempt = response.value;
                                break;
                            case 1:
                                r.ThirdAttempt = response.value;
                                break;
                        }
                        var isCorrect = r.Expected.Equals(response.value);
                        jsonReply.Add(new { id = response.id, correct = isCorrect, trysLeft = p.TrysRemaining - 1 });
                    }
                    p.TrysRemaining -= 1;
                    db.SaveChanges();
                }
                return Json(jsonReply, JsonRequestBehavior.AllowGet);
            }
        }
        public class JsonResponseObject
        {
            public int id { get; set; }
            public double value { get; set; }
        }
    }
}