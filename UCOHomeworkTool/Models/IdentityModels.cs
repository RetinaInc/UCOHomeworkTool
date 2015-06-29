using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;

namespace UCOHomeworkTool.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FirstAndLastName
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }
    }
    public class Teacher : ApplicationUser
    {
        public virtual List<Course> CoursesTeaching { get; set; }

    }
    public class Student : ApplicationUser
    {
        public virtual List<Course> CoursesTaking { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Assignment>().HasMany<Problem>(a => a.Problems).WithOptional().WillCascadeOnDelete(true);
            modelBuilder.Entity<Problem>().HasMany<Given>(p => p.Givens).WithOptional(g => g.Problem).WillCascadeOnDelete(true);
            modelBuilder.Entity<Problem>().HasMany<Response>(p => p.Responses).WithOptional(r => r.Problem).WillCascadeOnDelete(true);
        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<ProblemDiagram> ProblemDiagrams { get; set; }
        public DbSet<Given> Givens { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
    }
}