namespace UCOHomeworkTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTemplatesAndStudentToAssignment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Assignments", "Course_Id", "dbo.Courses");
            AddColumn("dbo.Assignments", "Course_Id1", c => c.Int());
            AddColumn("dbo.Assignments", "Course_Id2", c => c.Int());
            AddColumn("dbo.Assignments", "Student_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Givens", "minRange", c => c.Double());
            AddColumn("dbo.Givens", "maxRange", c => c.Double());
            AddColumn("dbo.Givens", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Assignments", "Course_Id1");
            CreateIndex("dbo.Assignments", "Course_Id2");
            CreateIndex("dbo.Assignments", "Student_Id");
            AddForeignKey("dbo.Assignments", "Course_Id1", "dbo.Courses", "Id");
            AddForeignKey("dbo.Assignments", "Student_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Assignments", "Course_Id2", "dbo.Courses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Assignments", "Course_Id2", "dbo.Courses");
            DropForeignKey("dbo.Assignments", "Student_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Assignments", "Course_Id1", "dbo.Courses");
            DropIndex("dbo.Assignments", new[] { "Student_Id" });
            DropIndex("dbo.Assignments", new[] { "Course_Id2" });
            DropIndex("dbo.Assignments", new[] { "Course_Id1" });
            DropColumn("dbo.Givens", "Discriminator");
            DropColumn("dbo.Givens", "maxRange");
            DropColumn("dbo.Givens", "minRange");
            DropColumn("dbo.Assignments", "Student_Id");
            DropColumn("dbo.Assignments", "Course_Id2");
            DropColumn("dbo.Assignments", "Course_Id1");
            AddForeignKey("dbo.Assignments", "Course_Id", "dbo.Courses", "Id");
        }
    }
}
