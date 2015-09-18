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
using System.Data.Entity.Validation;
using System.IO;
using System.Web.Script.Serialization;
using System.Globalization;

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
                var user = db.Teachers.Find(userId);
                var userCourses = user.CoursesTeaching;
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
                var course= db.Courses.Find(id);
                db.Entry(course).Collection("Templates").Load();
                db.Entry(course).Collection("Assignments").Load();
                return View(course);
            }

        }
        public ActionResult Enrollment(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var course= db.Courses.Find(id);
                db.Entry(course).Collection("Templates").Load();
                db.Entry(course).Collection("Assignments").Load();
                db.Entry(course).Collection("Students").Load();
                return View(course);
            }

        }
        public ActionResult AssignProblems(List<int> probids, int assignmentId, string dateTimeString)
        {
            DateTime dueDate = getDateTimeFromString(dateTimeString);
            if(probids == null)
            {
                probids = new List<int>();
            }
            using (var db = new ApplicationDbContext())
            {
                var assignment = db.Assignments.Include("Course").SingleOrDefault(x => x.Id == assignmentId);
                assignment.DueDate = dueDate;
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
        public ActionResult EnrollStudent(string studentId, int courseId)
        {
            string returnMessage;
            Object addedStudent = null;
            using(var db = new ApplicationDbContext())
            {
                //get the course object from the database
                var course = db.Courses.Find(courseId);
                //get the student object if it exists from the db
                var student = db.Students.Where(s => s.UserName.Equals(studentId, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                //handle case where student isnt registered
                if (student == null)
                {
                    returnMessage = "The student was not found, please notify the student to register";
                }
                //enroll student
                else
                {
                    var studentAlreadyEnrolled = course.EnrollStudent(db,student);
                    if(studentAlreadyEnrolled)
                    {
                        returnMessage = "The student with id " + studentId + " is already enrolled in this course"; 
                    }
                    else
                    {

                        addedStudent = new {LastName=student.LastName, FirstName=student.FirstName, StudentId=student.UserName, Id = student.Id};
                        returnMessage = "The student with id " + studentId + " was enrolled successfully";
                    }
                }
            }

            return Json(new { message = returnMessage, student = addedStudent }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UnenrollStudent(int courseId, string studentId)
        {
            string returnMessage;
            bool unenrollSuccess = false;
            using(var db = new ApplicationDbContext())
            {
                var course = db.Courses.Find(courseId);
                var student = db.Students.Find(studentId);
                if(student == null)
                {
                    returnMessage = "The student you tried to unenroll does not exist in the system";
                }
                else
                {
                    unenrollSuccess = course.UnenrollStudent(db, student);
                    if(unenrollSuccess)
                    {
                        returnMessage = "The student with id " + student.UserName + " was unenrolled successfully";
                    }
                    else
                    {
                        returnMessage = "The student with id " + student.UserName + " does not exist in this course. Please refresh your page.";
                    }
                }
            }

            return Json(new { message = returnMessage, success = unenrollSuccess }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Grades(int id)
        {
            using(var db = new ApplicationDbContext())
            {
                var course = db.Courses.Find(id);
                db.Entry(course).Collection("Templates").Load();
                db.Entry(course).Collection("Assignments").Load();
                foreach(var assignment in course.Assignments)
                {
                    db.Entry(assignment).Reference("Student").Load();
                }
                return View(course);
            }
        }
        public ActionResult Statistics(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var course = db.Courses.Find(id);
                var statModel = course.GetStatistics();
                statModel.Course = course;
                db.Entry(course).Collection("Assignments").Load();
                return View(statModel);
            }
        }
        public ActionResult ExportGrades(int courseId)
        {
            using(var db = new ApplicationDbContext())
            {
                var course = db.Courses.Find(courseId);
                if(course.Assignments.Count > 0)
                {
                    var memoryStream = course.ExportGrades();
                    return File(memoryStream, 
                                  "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                                  string.Format("{0}Grades.xlsx",course.Name.Replace(" ","")));
                }
                //if we got this far there are no grades to export, return a redirect to course page to take no action
                return RedirectToAction("Course", "Teacher", new { id = courseId });
            }
        }
        public ActionResult ExportStatistics(int courseId)
        {
            using (var db = new ApplicationDbContext())
            {
                var course = db.Courses.Find(courseId);
                if (course.Assignments.Count > 0)
                {
                    var memoryStream = course.ExportStats();
                    return File(memoryStream,
                                  "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                  string.Format("{0}Stats.xlsx", course.Name.Replace(" ", "")));
                }
                //if we got this far there are no stats to export, return a redirect to course page to take no action
                return RedirectToAction("Course", "Teacher", new { id = courseId });
            } 
        }
        [NonAction]
        public DateTime getDateTimeFromString(string dateTimeString)
        {
            return DateTime.Parse(dateTimeString);
        }
    }

}