using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCOHomeworkTool.Models;

namespace UCOHomeworkTool.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult Assignment(int id)
        {
            using (var database = new ApplicationDbContext())
            {
                var assignmentModel = database.Assignments.Include("Problems").SingleOrDefault(x => x.Id == id);
                var problemsModel = assignmentModel.Problems;
                foreach (var prob in problemsModel)
                {
                    database.Entry(prob).Collection("Givens").Load();
                    database.Entry(prob).Collection("Responses").Load();
                }
                return View(problemsModel.ToList());
            }
        }
        public ActionResult _Sidebar()
        {
            using (var db = new ApplicationDbContext())
            {
                var assignmentList = db.Assignments.ToList();
                return PartialView(assignmentList);
            }
        }
    }
}