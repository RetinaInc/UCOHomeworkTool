using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace UCOHomeworkTool.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema:false)
        {
        }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<Given> Givens { get; set; }
        public DbSet<Response> Responses { get; set; }
    }
}