namespace UCOHomeworkTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class implementCascadingDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Problems", "Assignment_Id", "dbo.Assignments");
            DropForeignKey("dbo.Givens", "Problem_Id", "dbo.Problems");
            DropForeignKey("dbo.Responses", "Problem_Id", "dbo.Problems");
            AddForeignKey("dbo.Problems", "Assignment_Id", "dbo.Assignments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Givens", "Problem_Id", "dbo.Problems", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Responses", "Problem_Id", "dbo.Problems", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Responses", "Problem_Id", "dbo.Problems");
            DropForeignKey("dbo.Givens", "Problem_Id", "dbo.Problems");
            DropForeignKey("dbo.Problems", "Assignment_Id", "dbo.Assignments");
            AddForeignKey("dbo.Responses", "Problem_Id", "dbo.Problems", "Id");
            AddForeignKey("dbo.Givens", "Problem_Id", "dbo.Problems", "Id");
            AddForeignKey("dbo.Problems", "Assignment_Id", "dbo.Assignments", "Id");
        }
    }
}
