namespace UCOHomeworkTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMoreAttemptsToProblems : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Responses", "FourthAttempt", c => c.Double(nullable: false));
            AddColumn("dbo.Responses", "FifthAttempt", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Responses", "FifthAttempt");
            DropColumn("dbo.Responses", "FourthAttempt");
        }
    }
}
