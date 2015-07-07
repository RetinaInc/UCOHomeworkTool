namespace UCOHomeworkTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeProblemIdtoProblemInProblemDiagramClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProblemDiagrams", "Problem_Id", c => c.Int());
            CreateIndex("dbo.ProblemDiagrams", "Problem_Id");
            AddForeignKey("dbo.ProblemDiagrams", "Problem_Id", "dbo.Problems", "Id");
            DropColumn("dbo.ProblemDiagrams", "ProblemId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProblemDiagrams", "ProblemId", c => c.Int(nullable: false));
            DropForeignKey("dbo.ProblemDiagrams", "Problem_Id", "dbo.Problems");
            DropIndex("dbo.ProblemDiagrams", new[] { "Problem_Id" });
            DropColumn("dbo.ProblemDiagrams", "Problem_Id");
        }
    }
}
