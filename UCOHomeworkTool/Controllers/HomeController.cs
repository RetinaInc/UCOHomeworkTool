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
        public ActionResult Assignment()
        {
            using (var database = new ApplicationDbContext())
            {
                var problemsModel = database.Problems.Include("Givens").Include("Responses").ToList();
                problemsModel.ElementAt(0).Givens.Add(new Given { Label = "V", Value = 1.5 });
                problemsModel.ElementAt(0).Responses.Add(new Response{Label="I", Expected = 73});
                database.SaveChanges();
                return View(problemsModel);
            }
        }
    }
}