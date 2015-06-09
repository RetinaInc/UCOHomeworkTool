namespace UCOHomeworkTool.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
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
            context.Users.AddOrUpdate(u => u.UserName, myUser);
            context.SaveChanges();
            //set up teacher role and give this user that role
            using (var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext())))
            using (var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
            {
                if (!rm.RoleExists("Teacher"))
                {
                    var roleResult = rm.Create(new IdentityRole("Teacher"));
                    if (!roleResult.Succeeded)
                        throw new ApplicationException("Creating role Teacher failed with errors: " + roleResult.Errors);
                }
                if (!um.IsInRole(myUser.Id, "Teacher"))
                {
                    var userResult = um.AddToRole(myUser.Id, "Teacher");
                }
            }

            //create Signals course for myUser and enroll myUser in course
            var courses = new List<Course>
            {
                new Course {Name = "Signals", Assignments = new List<Assignment>(), Templates = new List<Assignment>()},
                new Course {Name = "TestCourse", Assignments = new List<Assignment>(), Templates = new List<Assignment>()},
                new Course {Name = "Electrical Science", Assignments = new List<Assignment>(), Templates = new List<Assignment>()},
            };
            courses.ForEach(c => context.Courses.AddOrUpdate(u => u.Name, c));
            context.Users.Find(myUser.Id).Courses = courses;
            context.SaveChanges();
            //create dummy students and enroll them in testCourse to test problem randomization funcionality

            var student1 = new ApplicationUser
            {
                UserName = "1",
                PasswordHash = passwordHash.HashPassword("pass"),
                Courses = new List<Course> { courses[1], courses[2] },
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var student2 = new ApplicationUser
            {
                UserName = "2",
                PasswordHash = passwordHash.HashPassword("pass"),
                Courses = new List<Course> { courses[1], courses[2] },
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var student3 = new ApplicationUser
            {
                UserName = "3",
                PasswordHash = passwordHash.HashPassword("pass"),
                Courses = new List<Course> { courses[1], courses[2] },
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            List<ApplicationUser> students = new List<ApplicationUser> { student1, student2, student3 };
            //create assignment templates to be used in Signals course
            var assignmentsTemplate = new List<Assignment>()
            {
                new Assignment(){AssignmentNumber = 1, Course = courses[2], Problems = new List<Problem>()},
            };
            var p1Givens = new List<Given>()
            {
                new GivenTemplate {Label = "Vs1", minRange = 1, maxRange = 10,},
                new GivenTemplate {Label = "Vs2", minRange = 1, maxRange = 10,},
                new GivenTemplate {Label = "Vs3", minRange = 1, maxRange = 10,},
            };
            var p2Givens = new List<Given>()
            {
                new GivenTemplate {Label = "Vs1", minRange = 1, maxRange = 10,},
                new GivenTemplate {Label = "R1", minRange = 1, maxRange = 10,},
                new GivenTemplate {Label = "R2", minRange = 1, maxRange = 10,},
                new GivenTemplate {Label = "R3", minRange = 1, maxRange = 10,},
                new GivenTemplate {Label = "a", minRange = 1, maxRange = 10,},
            };
            var p3Givens = new List<Given>()
            {
                new GivenTemplate {Label = "Vs1", minRange = 1, maxRange = 10,},
                new GivenTemplate {Label = "R1", minRange = 1, maxRange = 10,},
                new GivenTemplate {Label = "R2", minRange = 1, maxRange = 10,},
                new GivenTemplate {Label = "R3", minRange = 1, maxRange = 10,},
                new GivenTemplate {Label = "R4", minRange = 1, maxRange = 10,},
                new GivenTemplate {Label = "Is3", minRange = 1, maxRange = 10,},
            };
            var p4Givens = new List<Given>()
            {
                new GivenTemplate {Label = "Vs", minRange = 1, maxRange = 10,},
                new GivenTemplate {Label = "R1", minRange = 1, maxRange = 10,},
                new GivenTemplate {Label = "R2", minRange = 1, maxRange = 10,},
                new GivenTemplate {Label = "R3", minRange = 1, maxRange = 10,},
                new GivenTemplate {Label = "R4", minRange = 1, maxRange = 10,},
                new GivenTemplate {Label = "R5", minRange = 1, maxRange = 10,},
            };
            var p1Resp = new List<Response>()
            {
                new Response {Label = "v1"},
                new Response {Label = "v2"},
                new Response {Label = "v3"},
            };
            var p2Resp = new List<Response>()
            {
                new Response {Label = "Vx"},
                new Response {Label = "P"},
            };
            var p3Resp = new List<Response>()
            {
                new Response {Label = "i due to source Vs1"},
                new Response {Label = "i due to source Vs2"},
                new Response {Label = "i due to source Is3"},
                new Response {Label = "i total"},
            };
            var p4Resp = new List<Response>()
            {
                new Response {Label = "io (in microAmps)"},
            };

            var problems = new List<Problem>()
            {
                new Problem(){ProblemNumber = 1, Givens = p1Givens, Responses = p1Resp},
                new Problem(){ProblemNumber = 2, Givens = p2Givens, Responses = p2Resp},
                new Problem(){ProblemNumber = 3, Givens = p3Givens, Responses = p3Resp},
                new Problem(){ProblemNumber = 4, Givens = p4Givens, Responses = p4Resp},
            };
            context.Courses.Find(courses[2].Id).Templates.AddRange(assignmentsTemplate);
            assignmentsTemplate[0].Problems.AddRange(problems);
            assignmentsTemplate.ForEach(t => context.Assignments.AddOrUpdate(t));
            context.SaveChanges();
            //associate diagrams to the created problems
            string path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            var diagram = Image.FromFile(path + "\\HW1_Prob1.jpg");
            var diagram1 = Image.FromFile(path + "\\h1p1.png");
            var diagram2 = Image.FromFile(path + "\\h1p2.png");
            var diagram3 = Image.FromFile(path + "\\h1p3.png");
            var diagram4 = Image.FromFile(path + "\\h1p4.png");
            var problemDiagrams = new List<ProblemDiagram>()
            {
                new ProblemDiagram{Id = 1, Diagram = diagram1, ProblemId = assignmentsTemplate[0].Problems[0].Id },
                new ProblemDiagram{Id = 2, Diagram = diagram2, ProblemId = assignmentsTemplate[0].Problems[1].Id },
                new ProblemDiagram{Id = 3, Diagram = diagram3, ProblemId = assignmentsTemplate[0].Problems[2].Id },
                new ProblemDiagram{Id = 4, Diagram = diagram4, ProblemId = assignmentsTemplate[0].Problems[3].Id },
            };
            problemDiagrams.ForEach(p => context.ProblemDiagrams.AddOrUpdate(p));
            //create a single problem to give to dummy students 
            var testProblem = new Problem
            {
                ProblemNumber = 1,
                Givens = new List<Given>(),
                Responses = new List<Response>(),
            };
            testProblem.Givens.Add(new GivenTemplate { Label = "R1", minRange = 1, maxRange = 10 });
            testProblem.Givens.Add(new GivenTemplate { Label = "R2", minRange = 1, maxRange = 10 });
            testProblem.Givens.Add(new GivenTemplate { Label = "R3", minRange = 1, maxRange = 10 });
            testProblem.Givens.Add(new GivenTemplate { Label = "V1", minRange = 1, maxRange = 10 });
            testProblem.Givens.Add(new GivenTemplate { Label = "V2", minRange = 1, maxRange = 10 });
            testProblem.Responses.Add(new Response { Label = "i1" });
            testProblem.Responses.Add(new Response { Label = "i2" });
            testProblem.Responses.Add(new Response { Label = "i3" });
            //create assignment for that problem
            var testAssignment = new Assignment
            {
                AssignmentNumber = 1,
                Course = courses[1],
                Problems = new List<Problem> { testProblem },
            };
            context.Courses.Find(courses[1].Id).Templates.Add(testAssignment);
            context.Assignments.AddOrUpdate(testAssignment);
            context.SaveChanges();
            //set up diagram for test problem
            var testDiagram = new ProblemDiagram
            {
                Id = 5,
                ProblemId = testProblem.Id,
                Diagram = diagram,
            };
            context.ProblemDiagrams.AddOrUpdate(p => p.Id, testDiagram);
            // instantiate students to db and connect test assignments to students
            foreach (var student in students)
            {
                var testAssignFromTemplate = new Assignment(testAssignment);
                context.Users.AddOrUpdate(student);
                testAssignFromTemplate.Student = student;
            }
            context.SaveChanges();
        }
    }
}
