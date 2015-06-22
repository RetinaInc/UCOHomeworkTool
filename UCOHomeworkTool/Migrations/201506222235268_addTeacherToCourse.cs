namespace UCOHomeworkTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTeacherToCourse : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationUserCourses", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserCourses", "Course_Id", "dbo.Courses");
            DropIndex("dbo.ApplicationUserCourses", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserCourses", new[] { "Course_Id" });
            AddColumn("dbo.Courses", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Courses", "Teacher_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "Course_Id", c => c.Int());
            CreateIndex("dbo.Courses", "ApplicationUser_Id");
            CreateIndex("dbo.Courses", "Teacher_Id");
            CreateIndex("dbo.AspNetUsers", "Course_Id");
            AddForeignKey("dbo.Courses", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "Course_Id", "dbo.Courses", "Id");
            AddForeignKey("dbo.Courses", "Teacher_Id", "dbo.AspNetUsers", "Id");
            DropTable("dbo.ApplicationUserCourses");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ApplicationUserCourses",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Course_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Course_Id });
            
            DropForeignKey("dbo.Courses", "Teacher_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.Courses", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "Course_Id" });
            DropIndex("dbo.Courses", new[] { "Teacher_Id" });
            DropIndex("dbo.Courses", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.AspNetUsers", "Course_Id");
            DropColumn("dbo.Courses", "Teacher_Id");
            DropColumn("dbo.Courses", "ApplicationUser_Id");
            CreateIndex("dbo.ApplicationUserCourses", "Course_Id");
            CreateIndex("dbo.ApplicationUserCourses", "ApplicationUser_Id");
            AddForeignKey("dbo.ApplicationUserCourses", "Course_Id", "dbo.Courses", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationUserCourses", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
