using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UCOHomeworkTool.Models;

namespace UCOHomeworkTool.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AssignmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Assignments
        public ActionResult Index()
        {
            return View(db.Assignments.ToList().OrderBy(a => a.Course.Name).ThenBy(a => a.AssignmentNumber));
        }

        // GET: Assignments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignments.Find(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // GET: Assignments/Create
        public ActionResult Create()
        {
            return View(new EditAssignmentViewModel());
        }

        // POST: Assignments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EditAssignmentViewModel assignment)
        {
            if (ModelState.IsValid)
            {
                var course = db.Courses.Where(c => c.Name.Equals(assignment.Course)).FirstOrDefault();
                var toAdd = new Assignment
                {
                    AssignmentNumber = assignment.AssignmentNumber,
                    Problems = assignment.Problems.Select(p => new Problem
                    {
                        ProblemNumber = p.ProblemNumber,
                        Description = p.Description,
                        ImageData = p.Diagram,
                        Givens = p.Givens.Select(g => new GivenTemplate
                        {
                            Label = g.Label,
                            minRange = g.MinValue,
                            maxRange = g.MaxValue,
                        }).Cast<Given>().ToList(),
                        Responses = p.Responses.Select(r => new Response
                        {
                            Label = r.Label,
                        }).ToList(),
                    }).ToList(),
                    Course = course,
                };
                //handle problem diagrams
                if (course != null)
                {
                    course.Templates.Add(toAdd);
                }
                db.Assignments.Add(toAdd);
                db.SaveChanges();
                foreach (var prob in toAdd.Problems)
                {
                    if (prob.ImageData != null)
                    {
                        var image = Image.FromStream(prob.ImageData.InputStream, true, true);
                        var diagram = new ProblemDiagram { Diagram = image, Problem = prob };
                        db.ProblemDiagrams.Add(diagram);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }

            return View(assignment);
        }

        // GET: Assignments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignments.Find(id);
            var viewModel = new EditAssignmentViewModel()
            {
                AssignmentNumber = assignment.AssignmentNumber,
                Problems = assignment.Problems.Select(p => new EditProblemViewModel
                {
                    Id = p.Id,
                    Description = p.Description,
                    ProblemNumber = p.ProblemNumber,
                    Givens = p.Givens.Select(g => new EditGivenViewModel
                    {
                        Id = g.Id,
                        Label = g.Label,
                        MinValue = ((GivenTemplate)g).minRange,
                        MaxValue = ((GivenTemplate)g).maxRange,
                    }).ToList(),
                    Responses = p.Responses.Select(r => new EditResponseViewModel
                    {
                        Label = r.Label,
                    }).ToList(),
                }
                ).ToList(),
                Course = assignment.Course.Name,
            };
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(viewModel);
        }

        // POST: Assignments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditAssignmentViewModel assignment)
        {
            if (ModelState.IsValid)
            {
                var dbAssignment = db.Assignments.Find(assignment.Id);
                //search for problems that need to be deleted
                var toDelete = dbAssignment.Problems.Select(d => d.Id).Except(assignment.Problems.Select(p => p.Id)).ToList();
                foreach (var id in toDelete)
                {
                    db.Problems.Remove(db.Problems.Find(id));
                }
                dbAssignment.AssignmentNumber = assignment.AssignmentNumber;
                var course = db.Courses.Where(c => c.Name.Equals(assignment.Course)).FirstOrDefault();
                foreach (var problem in assignment.Problems)
                {
                    if (problem.Id == 0)
                    {
                        var toAdd = new Problem
                        {
                            ProblemNumber = problem.ProblemNumber,
                            Description = problem.Description,
                            ImageData = problem.Diagram,
                            Givens = problem.Givens.Select(g => new GivenTemplate
                            {
                                Label = g.Label,
                                minRange = g.MinValue,
                                maxRange = g.MaxValue,
                            }).Cast<Given>().ToList(),
                            Responses = problem.Responses.Select(r => new Response
                            {
                                Label = r.Label,

                            }).ToList(),
                        };
                        dbAssignment.Problems.Add(toAdd);
                        db.SaveChanges();
                        if (problem.Diagram != null)
                        {
                            var image = Image.FromStream(toAdd.ImageData.InputStream, true, true);
                            var diagram = new ProblemDiagram { Diagram = image, Problem = toAdd };
                            db.ProblemDiagrams.Add(diagram);
                            db.SaveChanges();
                        }

                    }
                    else
                    {
                        var probToUpdate = dbAssignment.Problems.Where(p => p.Id == problem.Id).FirstOrDefault();
                        probToUpdate.Description = problem.Description;
                        probToUpdate.ProblemNumber = problem.ProblemNumber;
                        //establish new givens from viewmodel
                        probToUpdate.Givens.Clear();
                        probToUpdate.Givens.AddRange(problem.Givens.Select(g => new GivenTemplate
                            {
                                Id = g.Id,
                                Label = g.Label,
                                minRange = g.MinValue,
                                maxRange = g.MaxValue,
                            }).Cast<Given>().ToList());
                        //establish new responses from viewmodel
                        probToUpdate.Responses.Clear();
                        probToUpdate.Responses.AddRange(problem.Responses.Select(r => new Response
                           {
                               Id = r.Id,
                               Label = r.Label,
                           }).ToList());
                        //remove orphans
                        db.SaveChanges();
                        db.Givens.RemoveRange(db.Givens.Where(g => g.Problem == null));
                        //handle diagram
                        if(problem.Diagram != null)
                        {
                            //remove existing diagram
                            var existingDiagram = db.ProblemDiagrams.Where(d => d.Problem.Id == probToUpdate.Id).FirstOrDefault();
                            db.ProblemDiagrams.Remove(existingDiagram);
                            //create new diagram
                            var image = Image.FromStream(problem.Diagram.InputStream, true, true);
                            var diagram = new ProblemDiagram { Diagram = image, Problem = probToUpdate};
                            db.ProblemDiagrams.Add(diagram);
                            db.SaveChanges();
                        }
                    }
                }
                if (!dbAssignment.Course.Name.Equals(assignment.Course))
                {
                    dbAssignment.Course.Templates.Remove(dbAssignment);
                    course.Templates.Add(dbAssignment);
                    dbAssignment.Course = course;
                }
                course.Templates.Add(dbAssignment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(assignment);
        }

        // GET: Assignments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignments.Find(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // POST: Assignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Assignment assignment = db.Assignments.Find(id);
            db.Assignments.Remove(assignment);
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
        public ActionResult GetProblemEditor(string collectionIndex, int? pno)
        {
            ViewData["index"] = collectionIndex;
            return PartialView("Editors/_ProblemEditor", new EditProblemViewModel() {ProblemNumber = (int)pno });
        }
        public ActionResult GetGivenEditor(string collectionIndex)
        {
            ViewData["index"] = collectionIndex;
            return PartialView("Editors/_GivenEditor", new EditGivenViewModel());
        }
        public ActionResult GetResponseEditor(string collectionIndex)
        {
            ViewData["index"] = collectionIndex;
            return PartialView("Editors/_ResponseEditor", new EditResponseViewModel());
        }

    }
}
