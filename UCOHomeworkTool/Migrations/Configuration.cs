namespace UCOHomeworkTool.Migrations
{
    using Microsoft.AspNet.Identity;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using UCOHomeworkTool.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<UCOHomeworkTool.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(UCOHomeworkTool.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //clear database maunally because not doing so causes AddOrUpdate to throw error from not using unique identifiers
            //Unique identifiers (keys) cannot be used without generating them manually, its easier to just let EF handle that.

            context.Database.ExecuteSqlCommand("delete from Responses");
            context.Database.ExecuteSqlCommand("delete from Givens");
            context.Database.ExecuteSqlCommand("delete from ProblemDiagrams");
            context.Database.ExecuteSqlCommand("delete from Problems");
            context.Database.ExecuteSqlCommand("delete from Assignments");
            context.Database.ExecuteSqlCommand("delete from Courses");
            context.Database.ExecuteSqlCommand("delete from ApplicationUserCourses");
            context.Database.ExecuteSqlCommand("delete from AspNetUsers");
            
            //set up myUser for testing
            var passwordHash = new PasswordHasher();
            var myUser = new ApplicationUser
            {
                UserName = "20342421",
                PasswordHash = passwordHash.HashPassword("password"),
                Courses = new List<Course>(),
                SecurityStamp = Guid.NewGuid().ToString(),
            };



            //create Signals course for myUser and enroll myUser in course
            var courses = new List<Course>
            {
                new Course {Name = "Signals", Assignments = new List<Assignment>(), Templates = new List<Assignment>()},
                new Course {Name = "TestCourse", Assignments = new List<Assignment>(), Templates = new List<Assignment>()}
            };
            courses.ForEach(c => context.Courses.AddOrUpdate(u => u.Name, c));
            context.Users.AddOrUpdate(u => u.UserName, myUser);
            context.Users.Find(myUser.Id).Courses = courses;
            //create dummy students and enroll them in testCourse to test problem randomization funcionality

            var student1 = new ApplicationUser
            {
                UserName = "1",
                PasswordHash = passwordHash.HashPassword("pass"),
                Courses = new List<Course> {courses[1] },
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var student2 = new ApplicationUser
            {
                UserName = "2",
                PasswordHash = passwordHash.HashPassword("pass"),
                Courses = new List<Course> {courses[1] },
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var student3 = new ApplicationUser
            {
                UserName = "3",
                PasswordHash = passwordHash.HashPassword("pass"),
                Courses = new List<Course> {courses[1] },
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            List<ApplicationUser> students = new List<ApplicationUser> { student1, student2, student3 };
            //create assignment templates to be used in Signals course
            var assignments = new List<Assignment>
            {
                new Assignment {AssignmentNumber = 1, Problems = new List<Problem>(), Course = courses[0]},
                new Assignment {AssignmentNumber = 2, Problems = new List<Problem>(), Course = courses[0]},
                new Assignment {AssignmentNumber = 3, Problems = new List<Problem>(), Course = courses[0]},
                new Assignment {AssignmentNumber = 4, Problems = new List<Problem>(), Course = courses[0]},
            };
            assignments.ForEach(a => context.Assignments.AddOrUpdate(u => u.AssignmentNumber, a));
            //context.Courses.Find(courses[0].Id).Templates.AddRange(assignments);
            context.SaveChanges();
            var problems = new List<Problem>();
            foreach (var assignment in assignments)
            {
                var localProblems = new List<Problem>
                {
                    new Problem {ProblemNumber = 1, Givens = new List<Given>(), Responses = new List<Response>()},
                    new Problem {ProblemNumber = 2, Givens = new List<Given>(), Responses = new List<Response>()},
                    new Problem {ProblemNumber = 3, Givens = new List<Given>(), Responses = new List<Response>()},
                    new Problem {ProblemNumber = 4, Givens = new List<Given>(), Responses = new List<Response>()},
                    new Problem {ProblemNumber = 5, Givens = new List<Given>(), Responses = new List<Response>()},
                    new Problem {ProblemNumber = 6, Givens = new List<Given>(), Responses = new List<Response>()},
                };
                assignment.Problems.AddRange(localProblems);
                problems.AddRange(localProblems);
            }
            //remove some problems from some assignments to make it more visually obvious that the assignments are different
            assignments[1].Problems.RemoveAt(5);
            assignments[1].Problems.RemoveAt(4);
            assignments[3].Problems.RemoveAt(5);
            assignments[3].Problems.RemoveAt(4);
            assignments[3].Problems.RemoveAt(3);
            problems.ForEach(p => context.Problems.AddOrUpdate(u => u.ProblemNumber, p));
            context.SaveChanges();
            var givens = new List<Given>();
            foreach(var problem in problems)
            {
                var localGivens = new List<Given>();
                for(int i = 0; i < 5; i++)
                {
                    localGivens.Add(new GivenTemplate { Label = "P" + i + problem.ProblemNumber , minRange = 1, maxRange = 10 });
                }
                problem.Givens.AddRange(localGivens);
                givens.AddRange(localGivens);
            }
            givens.ForEach(g => context.Givens.AddOrUpdate(u => u.Label, g));
            context.SaveChanges();
            var responses = new List<Response>();
            foreach( var problem in problems)
            {
                var localResp = new List<Response>();
                for (int i = 0; i < 2; i++)
                {
                    localResp.Add(new Response { Label = "A" + i + problem.ProblemNumber, Expected = 4.4  });
                }
                problem.Responses.AddRange(localResp);
                responses.AddRange(localResp);                
            }
            responses.ForEach(r => context.Responses.AddOrUpdate(u => u.Label, r));

            //associate diagrams to the created problems
            string path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            var diagram = Image.FromFile(path + "\\HW1_Prob1.jpg");
            int id = 1;
            foreach(var prob in problems)
            {
                var problemDiagram = new ProblemDiagram
                {
                    Id = id++,
                    Diagram = diagram,
                    ProblemId = prob.Id,
                };
                context.ProblemDiagrams.AddOrUpdate(p => p.Id, problemDiagram);
            }

            //use template assignment to create assignment for myUser
            var assignmentsForMyUser = new List<Assignment>();
            foreach(var assignment in assignments)
            {
                var newAssignment = new Assignment(assignment);
                newAssignment.Student = myUser;
                assignmentsForMyUser.Add(newAssignment);
            }
            context.Courses.Find(courses[0].Id).Assignments.AddRange(assignmentsForMyUser);
            context.SaveChanges();

            //create a single problem to give to dummy students 
            var testProblem = new Problem
            {
                ProblemNumber = 1, Givens = new List<Given>(), Responses = new List<Response>(),
            };
            testProblem.Givens.Add(new GivenTemplate { Label = "R1", minRange = 1, maxRange = 10 });
            testProblem.Givens.Add(new GivenTemplate { Label = "R2", minRange = 1, maxRange = 10 });
            testProblem.Givens.Add(new GivenTemplate { Label = "R3", minRange = 1, maxRange = 10 });
            testProblem.Givens.Add(new GivenTemplate { Label = "V1", minRange = 1, maxRange = 10 });
            testProblem.Givens.Add(new GivenTemplate { Label = "V2", minRange = 1, maxRange = 10 });
            testProblem.Responses.Add(new Response { Label = "i1"});
            testProblem.Responses.Add(new Response { Label = "i2"});
            testProblem.Responses.Add(new Response { Label = "i3"});
            //create assignment for that problem
            var testAssignment = new Assignment
            {
                AssignmentNumber = 1,
                Course = courses[1],
                Problems = new List<Problem>{testProblem},
            };
            context.Courses.Find(courses[1].Id).Templates.Add(testAssignment);
            context.Assignments.AddOrUpdate(testAssignment);
            context.SaveChanges();
            //set up diagram for test problem
            var testDiagram = new ProblemDiagram
            {
                Id = id++,
                ProblemId = testProblem.Id,
                Diagram = diagram,
            };
            context.ProblemDiagrams.AddOrUpdate(p => p.Id, testDiagram);
            // instantiate students to db and connect test assignments to students
            foreach(var student in students)
            {
                var testAssignFromTemplate = new Assignment(testAssignment);
                context.Users.AddOrUpdate(student);
                testAssignFromTemplate.Student = student;
                context.Assignments.AddOrUpdate(testAssignFromTemplate);
            }
            context.SaveChanges();
        }
    }
}
