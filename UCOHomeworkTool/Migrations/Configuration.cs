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
            ContextKey = "UCOHomeworkTool.Models.ApplicationDbContext";
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
            var problems = new List<Problem>
            {
                new Problem {ProblemNumeber = 1, Responses = new List<Response>(), Givens = new List<Given>()},
                new Problem {ProblemNumeber = 2, Responses = new List<Response>(), Givens = new List<Given>()},
                new Problem {ProblemNumeber = 5, Responses = new List<Response>(), Givens = new List<Given>()},
            };
            problems.ForEach(p => context.Problems.AddOrUpdate(p));
            context.SaveChanges();

            var givens = new List<Given>
            {
                new Given {Label = "v", Value = 15.2 },
                new Given {Label = "i", Value = 1.23},
                new Given {Label = "O", Value = 12.1},
            };
            givens.ForEach(g => context.Givens.AddOrUpdate(g));
            context.SaveChanges();

            var responses = new List<Response>
            {
                new Response {Label = "i", Expected = 13.2},
                new Response {Label = "p", Expected = 13.1},
                new Response {Label = "r", Expected = 13.6},
                new Response {Label = "dy/dx", Expected = 1.2},
                
            };
            responses.ForEach(r => context.Responses.AddOrUpdate(r));
            context.SaveChanges();

            problems[1].Givens.AddRange(givens);
            problems[1].Responses.AddRange(responses);
            context.SaveChanges();

        }
    }
}
