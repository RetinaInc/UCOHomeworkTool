namespace UCOHomeworkTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeProblemRequiredInProblemDiagram : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProblemDiagrams", "Problem_Id", "dbo.Problems");
            DropIndex("dbo.ProblemDiagrams", new[] { "Problem_Id" });
            AlterColumn("dbo.ProblemDiagrams", "Problem_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.ProblemDiagrams", "Problem_Id");
            AddForeignKey("dbo.ProblemDiagrams", "Problem_Id", "dbo.Problems", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProblemDiagrams", "Problem_Id", "dbo.Problems");
            DropIndex("dbo.ProblemDiagrams", new[] { "Problem_Id" });
            AlterColumn("dbo.ProblemDiagrams", "Problem_Id", c => c.Int());
            CreateIndex("dbo.ProblemDiagrams", "Problem_Id");
            AddForeignKey("dbo.ProblemDiagrams", "Problem_Id", "dbo.Problems", "Id");
        }
    }
}
