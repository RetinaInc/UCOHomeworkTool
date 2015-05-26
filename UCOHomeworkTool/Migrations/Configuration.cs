namespace UCOHomeworkTool.Migrations
{
    using Microsoft.AspNet.Identity;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
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
            };
            courses.ForEach(c => context.Courses.AddOrUpdate(u => u.Name, c));
            context.Users.AddOrUpdate(u => u.UserName, myUser);
            context.Users.Find(myUser.Id).Courses = courses;

            //create assignment templates to be used in Signals course
            var assignments = new List<Assignment>
            {
                new Assignment {AssignmentNumber = 1, Problems = new List<Problem>(), Course = courses[0]},
                new Assignment {AssignmentNumber = 2, Problems = new List<Problem>(), Course = courses[0]},
                new Assignment {AssignmentNumber = 3, Problems = new List<Problem>(), Course = courses[0]},
                new Assignment {AssignmentNumber = 4, Problems = new List<Problem>(), Course = courses[0]},
            };
            assignments.ForEach(a => context.Assignments.AddOrUpdate(u => u.AssignmentNumber, a));
            context.Courses.Find(courses[0].Id).Templates.AddRange(assignments);
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
            problems.ForEach(p => context.Problems.AddOrUpdate(u => u.ProblemNumber, p));
            context.SaveChanges();
            var givens = new List<Given>();
            foreach(var problem in problems)
            {
                var localGivens = new List<Given>();
                for(int i = 0; i < 5; i++)
                {
                    localGivens.Add(new GivenTemplate { Label = "P" + i + problem.ProblemNumber , minRange = 1.5, maxRange = 8.4 });
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
                    localResp.Add(new Response { Label = "A" + i + problem.ProblemNumber  });
                }
                problem.Responses.AddRange(localResp);
                responses.AddRange(localResp);                
            }
            responses.ForEach(r => context.Responses.AddOrUpdate(u => u.Label, r));

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
        }
    }
}
