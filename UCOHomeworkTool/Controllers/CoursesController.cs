using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UCOHomeworkTool.Models;
using PagedList;

namespace UCOHomeworkTool.Controllers
{
    [Authorize(Roles="Admin")]
    public class CoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Courses
        public ActionResult Index()
        {
            int pageSize = 10;
            int pageNumber = 1;
            return View(db.Courses.OrderBy(course=> course.Name).ToPagedList(pageNumber, pageSize));
        }
        public PartialViewResult CourseTablePage(int page)
        {
            int pageSize = 10;
            return PartialView("_CourseTable", db.Courses.OrderBy(course => course.Name).ToPagedList(page, pageSize));
        }
        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            var teacherNames = db.Teachers.ToList().Select(t => new SelectListItem { Text = t.FirstAndLastName, Value = t.UserName }).ToList();
            ViewBag.TeacherNames = teacherNames;
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Teacher,CoursePrefix,CourseNumber")] Course course)
        {
            if (ModelState.IsValid)
            {
                var teacher = db.Teachers.Where(t => t.UserName == course.Teacher.UserName).FirstOrDefault();
                course.Teacher = teacher;
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }

            var teacherNames = db.Teachers.ToList().Select(t => new SelectListItem { Text = t.FirstAndLastName, Value = t.UserName }).ToList(); 
            ViewBag.TeacherNames = teacherNames;
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Teacher,CoursePrefix,CourseNumber")] Course course)
        {
            if (ModelState.IsValid)
            {
                var existingCourse = db.Courses.Include(c => c.Teacher).Where(c => c.Id == course.Id).SingleOrDefault();
                var teacher = db.Teachers.Where(t => t.UserName == course.Teacher.UserName).FirstOrDefault();
                existingCourse.CourseNumber = course.CourseNumber;
                existingCourse.CoursePrefix = course.CoursePrefix;
                if(teacher == null)
                {
                    existingCourse.Teacher = null;
                }
                else
                {
                    existingCourse.Teacher = teacher;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            var toRemove = new List<Assignment>();
            foreach(var template in course.Templates)
            {
                toRemove.Add(template);
            }
            foreach(var assignment in course.Assignments)
            {
                toRemove.Add(assignment);
            }
            db.Assignments.RemoveRange(toRemove);
            db.Courses.Remove(course);
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
