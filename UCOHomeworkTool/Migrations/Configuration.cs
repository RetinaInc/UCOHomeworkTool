namespace UCOHomeworkTool.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
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
            var courses = new List<Course>
            {
                new Course {Name = "Signals", Assignments = new List<Assignment>()},
            };
            courses.ForEach(c => context.Courses.AddOrUpdate(u => u.Name, c));

            var assignments = new List<Assignment>
            {
                new Assignment {AssignmentNumber = 1, Problems = new List<Problem>()},
                new Assignment {AssignmentNumber = 2, Problems = new List<Problem>()},
                new Assignment {AssignmentNumber = 3, Problems = new List<Problem>()},
                new Assignment {AssignmentNumber = 4, Problems = new List<Problem>()},
            };
            assignments.ForEach(a => context.Assignments.AddOrUpdate(u => u.AssignmentNumber, a));
            context.Courses.Find(1).Assignments.AddRange(assignments);
            context.SaveChanges();
            var problems = new List<Problem>();
            foreach (var assignment in assignments)
            {
                var localProblems = new List<Problem>
                {
                    new Problem {ProblemNumeber = 1, Givens = new List<Given>(), Responses = new List<Response>()},
                    new Problem {ProblemNumeber = 2, Givens = new List<Given>(), Responses = new List<Response>()},
                    new Problem {ProblemNumeber = 3, Givens = new List<Given>(), Responses = new List<Response>()},
                    new Problem {ProblemNumeber = 4, Givens = new List<Given>(), Responses = new List<Response>()},
                    new Problem {ProblemNumeber = 5, Givens = new List<Given>(), Responses = new List<Response>()},
                    new Problem {ProblemNumeber = 6, Givens = new List<Given>(), Responses = new List<Response>()},
                };
                assignment.Problems.AddRange(localProblems);
                problems.AddRange(localProblems);
            }
            problems.ForEach(p => context.Problems.AddOrUpdate(u => u.ProblemNumeber, p));
                
            context.SaveChanges();

            Random rand = new Random();

            var givens = new List<Given>();
            foreach(var problem in problems)
            {
                var localGivens = new List<Given>();
                for(int i = 0; i < 5; i++)
                {
                    localGivens.Add(new Given { Label = "P" + i + problem.ProblemNumeber , Value = rand.NextDouble() + rand.Next() });
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
                    localResp.Add(new Response { Label = "A" + i + problem.ProblemNumeber, Expected = rand.NextDouble() });
                }
                problem.Responses.AddRange(localResp);
                responses.AddRange(localResp);                
            }
            responses.ForEach(r => context.Responses.AddOrUpdate(u => u.Label, r));

            context.SaveChanges();
        }
    }
}
