namespace UCOHomeworkTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRequiredRelationships : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Assignments", "Course_Id2", "dbo.Courses");
            DropForeignKey("dbo.Givens", "Problem_Id", "dbo.Problems");
            DropForeignKey("dbo.Responses", "Problem_Id", "dbo.Problems");
            DropIndex("dbo.Assignments", new[] { "Course_Id2" });
            DropIndex("dbo.Givens", new[] { "Problem_Id" });
            DropIndex("dbo.Responses", new[] { "Problem_Id" });
            AlterColumn("dbo.Assignments", "Course_Id2", c => c.Int(nullable: false));
            AlterColumn("dbo.Givens", "Problem_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Responses", "Problem_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Assignments", "Course_Id2");
            CreateIndex("dbo.Givens", "Problem_Id");
            CreateIndex("dbo.Responses", "Problem_Id");
            AddForeignKey("dbo.Assignments", "Course_Id2", "dbo.Courses", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Givens", "Problem_Id", "dbo.Problems", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Responses", "Problem_Id", "dbo.Problems", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Responses", "Problem_Id", "dbo.Problems");
            DropForeignKey("dbo.Givens", "Problem_Id", "dbo.Problems");
            DropForeignKey("dbo.Assignments", "Course_Id2", "dbo.Courses");
            DropIndex("dbo.Responses", new[] { "Problem_Id" });
            DropIndex("dbo.Givens", new[] { "Problem_Id" });
            DropIndex("dbo.Assignments", new[] { "Course_Id2" });
            AlterColumn("dbo.Responses", "Problem_Id", c => c.Int());
            AlterColumn("dbo.Givens", "Problem_Id", c => c.Int());
            AlterColumn("dbo.Assignments", "Course_Id2", c => c.Int());
            CreateIndex("dbo.Responses", "Problem_Id");
            CreateIndex("dbo.Givens", "Problem_Id");
            CreateIndex("dbo.Assignments", "Course_Id2");
            AddForeignKey("dbo.Responses", "Problem_Id", "dbo.Problems", "Id");
            AddForeignKey("dbo.Givens", "Problem_Id", "dbo.Problems", "Id");
            AddForeignKey("dbo.Assignments", "Course_Id2", "dbo.Courses", "Id");
        }
    }
}
