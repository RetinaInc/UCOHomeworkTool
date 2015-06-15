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
    using UCOHomeworkTool.Calculations;

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
                Courses = new List<Course> { courses[2] },
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var student2 = new ApplicationUser
            {
                UserName = "2",
                PasswordHash = passwordHash.HashPassword("pass"),
                Courses = new List<Course> {courses[0], courses[1], courses[2] },
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var student3 = new ApplicationUser
            {
                UserName = "3",
                PasswordHash = passwordHash.HashPassword("pass"),
                Courses = new List<Course> { courses[0], courses[2] },
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            List<ApplicationUser> students = new List<ApplicationUser> { student1, student2, student3 };
            //create assignment templates to be used in Signals course
            var assignmentsTemplate = new List<Assignment>()
            {
                new Assignment(){AssignmentNumber = 1, Course = courses[0], Problems = new List<Problem>()},
            };
            var problems = new List<Problem>()
            {
                createProblem(1,
                              new List<string>{"Vs1", "Vs2","Vs3"},
                              new List<string>{"v1","v2","v3"},
                              null),
                createProblem(2,
                              new List<string>{"Vs1", "R1","R2","R3","a"},
                              new List<string>{"Vx","P"},
                              null),
                createProblem(3,
                              new List<string>{"Vs1","R1","R2","R3","R4","Is3"},
                              new List<string>{"i due to source Vs1","i due to source Vs2","i due to source Vs3", "i total"},
                              null),
                createProblem(4,
                              new List<string>{"Vs", "R1","R2","R3","R4","R5"},new List<string>{"io (in microAmps)"},null),
            };
            context.Courses.Find(courses[0].Id).Templates.AddRange(assignmentsTemplate);
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
                new ProblemDiagram{Diagram = diagram1, ProblemId = assignmentsTemplate[0].Problems[0].Id },
                new ProblemDiagram{Diagram = diagram2, ProblemId = assignmentsTemplate[0].Problems[1].Id },
                new ProblemDiagram{Diagram = diagram3, ProblemId = assignmentsTemplate[0].Problems[2].Id },
                new ProblemDiagram{Diagram = diagram4, ProblemId = assignmentsTemplate[0].Problems[3].Id },
            };
            problemDiagrams.ForEach(p => context.ProblemDiagrams.AddOrUpdate(p));
            //create a single problem to give to dummy students 
            var testProblem = createProblem(1, 
                                            new List<string> { "R1", "R2", "R3", "V1", "V2" }, 
                                            new List<string> { "i1", "i2", "i3" }, 
                                            new Response.CalculateResponseDelegate(Calculations.a5p1));
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
            createFourAssignmentsForESCourse(context);
        }
    private void createFourAssignmentsForESCourse(ApplicationDbContext context)
    {
        //find the Electrical Science Course
        var course = context.Courses.Where(c => c.Name.Equals("electrical science", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        //construct assignment 1, problems first
        var probs1 = new List<Problem>{
            createProblem(1, 
                          new List<string>{"Vs1","Vs2","Vs3"},
                          new List<string>{"v1","v2","v3"},
                          new Response.CalculateResponseDelegate(Calculations.a1p1)),
            createProblem(2,
                          new List<string>{"Vs1","R1","R2","R3","a"},
                          new List<string>{"Vx","P"},
                          new Response.CalculateResponseDelegate(Calculations.a1p2)),
            createProblem(3,
                          new List<string>{"Vs","R1","R2","R3","R4"},
                          new List<string>{"V0","I0"},
                          new Response.CalculateResponseDelegate(Calculations.a1p3)),
            createProblem(4,
                          new List<string>{"Vb","Ib", "R1", "R2"},
                          new List<string>{"Vs"},
                          new Response.CalculateResponseDelegate(Calculations.a1p4)),
        };
        var probs2 = new List<Problem>{
            createProblem(1, 
                          new List<string>{"Vs","R1","R2","R3","R4","a"},
                          new List<string>{"V0","Ix"},
                          new Response.CalculateResponseDelegate(Calculations.a2p1)),
            createProblem(2,
                          new List<string>{"Vs1","Vs2","Is","R1","R2","R3","R4"},
                          new List<string>{"i0","i1"},
                          new Response.CalculateResponseDelegate(Calculations.a2p2)),
            createProblem(3,
                          new List<string>{"Vs","Is","R1","R2","R3","R4","R5"},
                          new List<string>{"V1","V2","V3","V4"},
                          new Response.CalculateResponseDelegate(Calculations.a2p3)),
        };
        var probs3 = new List<Problem>{
            createProblem(1, 
                          new List<string>{"Vs1","Vs2","R1","R2","R3","R4","Is3"},
                          new List<string>{"i due to source Vs1","i due to source Vs2", "i due to source Is3", "i total"},
                          new Response.CalculateResponseDelegate(Calculations.a3p1)),
            createProblem(2,
                          new List<string>{"Vs1","Vs2","Is","R1","R2","R3","R4"},
                          new List<string>{"Vx"},
                          new Response.CalculateResponseDelegate(Calculations.a3p2)),
            createProblem(3,
                          new List<string>{"Vs","Is","R1","R2","R3","R4","R5"},
                          new List<string>{"Vth_ab","Rth_ab","Vth_bc","Rth_bc"},
                          new Response.CalculateResponseDelegate(Calculations.a3p3)),
        };
        var probs4 = new List<Problem>{
            createProblem(1, 
                          new List<string>{"Vs","R1","R2","R3","R4","R5"},
                          new List<string>{"io (in microAmp)"},
                          new Response.CalculateResponseDelegate(Calculations.a4p1)),
            createProblem(2,
                          new List<string>{"Vs1","Vs2","R1","R2","R3","R4","R5"},
                          new List<string>{"Vo"},
                          new Response.CalculateResponseDelegate(Calculations.a4p2)),
        };
        var assignment1 = new Assignment { AssignmentNumber = 1, Course = course, Problems = probs1 };
        var assignment2 = new Assignment { AssignmentNumber = 2, Course = course, Problems = probs2 };
        var assignment3 = new Assignment { AssignmentNumber = 3, Course = course, Problems = probs3 };
        var assignment4 = new Assignment { AssignmentNumber = 4, Course = course, Problems = probs4 };
        course.Templates.Add(assignment1);
        course.Templates.Add(assignment2);
        course.Templates.Add(assignment3);
        course.Templates.Add(assignment4);
        context.SaveChanges();
        //create problem diagrams
        var path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
        var diag11 = Image.FromFile(path + "\\a1p1.png");
        var diag12 = Image.FromFile(path + "\\a1p2.png");
        var diag13 = Image.FromFile(path + "\\a1p3.png");
        var diag14 = Image.FromFile(path + "\\a1p4.png");
        var diag21= Image.FromFile(path + "\\a2p1.png");
        var diag22= Image.FromFile(path + "\\a2p2.png");
        var diag23= Image.FromFile(path + "\\a2p3.png");
        var diag31= Image.FromFile(path + "\\a3p1.png");
        var diag32= Image.FromFile(path + "\\a3p2.png");
        var diag33= Image.FromFile(path + "\\a3p3.png");
        var diag41= Image.FromFile(path + "\\a4p1.png");
        var diag42= Image.FromFile(path + "\\a4p2.png");
        var diagrams = new List<ProblemDiagram>{
            new ProblemDiagram{Diagram = diag11, ProblemId = probs1[0].Id},
            new ProblemDiagram{Diagram = diag12, ProblemId = probs1[1].Id},
            new ProblemDiagram{Diagram = diag13, ProblemId = probs1[2].Id},
            new ProblemDiagram{Diagram = diag14, ProblemId = probs1[3].Id},
            new ProblemDiagram{Diagram = diag21, ProblemId = probs2[0].Id},
            new ProblemDiagram{Diagram = diag22, ProblemId = probs2[1].Id},
            new ProblemDiagram{Diagram = diag23, ProblemId = probs2[2].Id},
            new ProblemDiagram{Diagram = diag31, ProblemId = probs3[0].Id},
            new ProblemDiagram{Diagram = diag32, ProblemId = probs3[1].Id},
            new ProblemDiagram{Diagram = diag33, ProblemId = probs3[2].Id},
            new ProblemDiagram{Diagram = diag41, ProblemId = probs4[0].Id},
            new ProblemDiagram{Diagram = diag42, ProblemId = probs4[1].Id},
        };
        diagrams.ForEach(d => context.ProblemDiagrams.AddOrUpdate(d));
    }
    private List<Given> createGivensList(List<string> labels)
    {
        List<Given> givens = new List<Given>();
        foreach (var label in labels)
        {
            var givenTemplate = new GivenTemplate{Label = label, minRange = 1, maxRange = 10};
            givens.Add(givenTemplate);
        }
        return givens;

    }
    private List<Response> createResponseList(List<string> labels, Response.CalculateResponseDelegate calcDelegate)
    {
        var responses = new List<Response>();
        foreach(var label in labels)
        {
            var resp = new Response { Label = label, calculation = calcDelegate};
            responses.Add(resp);
        }
        return responses;
    }
    private Problem createProblem(int problemNumber, List<string> givenLabels, List<string> responseLabels, Response.CalculateResponseDelegate calcDelegate)
    {
        var problem = new Problem { ProblemNumber = problemNumber, 
                                    Givens = createGivensList(givenLabels), 
                                    Responses = createResponseList(responseLabels, calcDelegate) };
        return problem;
    }

    }
}
